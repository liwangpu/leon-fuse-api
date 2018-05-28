﻿using ApiModel.Entities;
using System;

namespace ApiServer.Data
{
    /// <summary>
    /// 数据库初始化工具
    /// </summary>
    public class DbInitializer
    {
        private static ApiDbContext context;

        /// <summary>
        /// 如果数据库为空（第一次运行），填充初始化数据
        /// </summary>
        /// <param name="dbContext"></param>
        public static void InitDbIfItsEmpty(ApiDbContext dbContext)
        {
            context = dbContext;

            try
            {
                InitUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void InitUsers()
        {
            //init admin
            Account admin = context.Accounts.Find(Services.ConstVar.AdminAccountId);
            if(admin == null)
            {
                admin = new Account();
                Account acc = admin;
                acc.Id = Services.ConstVar.AdminAccountId;
                acc.Mail = Services.ConstVar.AdminAccount;
                acc.Password = Services.ConstVar.DefaultAdminPasswordMd5;
                acc.Frozened = false;
                acc.ActivationTime = DateTime.UtcNow;
                acc.ExpireTime = DateTime.UtcNow.AddYears(100);
                acc.Type = "sysadmin";
                acc.Name = "超级管理员";
                context.Accounts.Add(acc);
            }

            context.SaveChangesAsync();
        }
    }
}
