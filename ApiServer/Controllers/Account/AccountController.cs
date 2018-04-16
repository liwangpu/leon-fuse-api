﻿using ApiModel.Entities;
using ApiServer.Models;
using ApiServer.Services;
using BambooCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    /// <summary>
    /// 账号相关的接口
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class AccountController : Controller
    {
        AccountMan accountMan;

        public AccountController(Data.ApiDbContext context)
        {
            accountMan = new AccountMan(context);
        }

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="value">新账号的基本信息</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        [Produces(typeof(Account))]
        public async Task<IActionResult> Register([FromBody]RegisterAccountModel value)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            AccountModel account = await accountMan.Register(value);
            if (account == null)
                return BadRequest();
            return Ok(account);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody]ResetPasswordModel value)
        {
            //if (ModelState.IsValid == false)
            //    return BadRequest(ModelState);
            return Ok();
        }

        /// <summary>
        /// 设置新密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("NewPassword")]
        [HttpPost]
        public async Task<IActionResult> NewPassword([FromBody]NewPasswordModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            bool bOk = await accountMan.ChangePasswordAsync(AuthMan.GetAccountId(this), value);
            if (bOk)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("Profile")]
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromBody]AccountProfileModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            bool bOk = await accountMan.UpdateProfile(AuthMan.GetAccountId(this), value);
            if (bOk)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <returns></returns>
        [Route("Profile")]
        [HttpGet]
        public async Task<AccountProfileModel> GetProfile()
        {
            return await accountMan.GetProfile(AuthMan.GetAccountId(this));
        }

        /// <summary>
        /// 获取这个账号的网站后台导航菜单配置
        /// </summary>
        /// <returns></returns>
        [Route("Navigation")]
        [HttpGet]
        public async Task<NavigationModel> GetNavigationData()
        {
            return await accountMan.GetNavigation(AuthMan.GetAccountId(this));
        }
    }
}
