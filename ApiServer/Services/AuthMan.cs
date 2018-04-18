using ApiModel.Entities;
using ApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiServer.Services
{
    public class AuthMan
    {
        public enum LoginResult
        {
            /// <summary>
            /// 登陆成功
            /// </summary>
            Ok,
            /// <summary>
            /// 账号或密码错误
            /// </summary>
            AccOrPasswordWrong,
            /// <summary>
            /// 账号被冻结
            /// </summary>
            Frozen,
            /// <summary>
            /// 账号没有激活
            /// </summary>
            NotActivation,
            /// <summary>
            /// 账号已经过期
            /// </summary>
            Expired
        }

        public struct LoginResultStruct
        {
            public LoginResult loginResult;
            public AccountModel acc;
        }

        Data.ApiDbContext context;
        Controller controller;
        public AuthMan(Controller controller, Data.ApiDbContext context)
        {
            this.controller = controller;
            this.context = context;
        }

        public async Task<LoginResultStruct> LoginRequest(string account, string pwd)
        {
            LoginResultStruct result = new LoginResultStruct();
            result.loginResult = LoginResult.Ok;
            result.acc = null;

            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd))
                return result;
            account = account.ToLower();
            pwd = pwd.ToLower();

            var acc = await context.Accounts.FirstOrDefaultAsync(d => d.Mail == account || d.Phone == account);
            if (acc != null)
            {
                var model = new AccountModel();
                model.Id = acc.Id;
                model.Frozened = acc.Frozened;
                model.ActivationTime = acc.ActivationTime;
                model.ExpireTime = acc.ExpireTime;
                model.Password = acc.Password;
                result.acc = model;
            }

            if (result.acc == null)
            {
                result.loginResult = LoginResult.AccOrPasswordWrong;
            }
            else
            {
                if (result.acc.Frozened)
                    result.loginResult = LoginResult.Frozen;

                var now = DateTime.UtcNow;
                if (now < result.acc.ActivationTime)
                    result.loginResult = LoginResult.NotActivation;

                if (now > result.acc.ExpireTime)
                    result.loginResult = LoginResult.Expired;

                if (result.acc.Password != pwd)
                    result.loginResult = LoginResult.AccOrPasswordWrong;
            }


            return result;
        }

        public static string GetAccountId(Controller c)
        {
            return c.User.Identity.Name;
        }

        public static Account GetAccount(Controller c, DbContext context)
        {
            return context.Set<Account>().Find(c.User.Identity.Name);
        }

    }
}
