using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class UserRoleController : Controller
    {
        protected readonly ApiDbContext _Context;

        #region 构造函数
        public UserRoleController(ApiDbContext context)
        {
            _Context = context;
        }
        #endregion

        #region _GetCurrentUserOrgan 获取当前用户的组织
        /// <summary>
        /// 获取当前用户的组织
        /// </summary>
        /// <returns></returns>
        protected async Task<Organization> _GetCurrentUserOrgan()
        {
            var accid = AuthMan.GetAccountId(this);
            var account = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
            if (account != null)
            {
                var organ = await _Context.Organizations.FirstOrDefaultAsync(x => x.Id == account.OrganizationId);
                return organ;
            }
            return null;
        }
        #endregion

        #region Get 获取用户角色信息
        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="organType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string organType)
        {
            var roles = new List<UserRole>();
            if (!string.IsNullOrWhiteSpace(organType))
                roles = await _Context.UserRoles.Where(x => x.Organization == organType).ToListAsync();
            else
                roles = await _Context.UserRoles.ToListAsync();
            return Ok(roles);
        } 
        #endregion




    }
}