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

namespace ApiServer.Controllers.UIDesigner
{
    [Authorize]
    [Route("/[controller]")]
    public class NavigationController : Controller
    {
        protected readonly ApiDbContext _Context;

        #region 构造函数
        public NavigationController(ApiDbContext context)
        {
            _Context = context;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Get(string role)
        {
            var nav = await _Context.Navigations.Where(x => x.Role == role).FirstOrDefaultAsync();
            if (nav != null)
                return Ok(nav.Navs);
            return Ok();
        }

        #region Post 创建或更新用户角色导航菜单
        /// <summary>
        /// 创建或更新用户角色导航菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] NavigationEditModel model)
        {
            var nav = await _Context.Navigations.Where(x => x.Role == model.Role).FirstOrDefaultAsync();
            if (nav != null)
            {
                nav.Navs = model.Navs;
                _Context.Update(nav);
            }
            else
            {
                nav = new Navigation();
                nav.Id = GuidGen.NewGUID();
                nav.Role = model.Role;
                nav.Navs = model.Navs;
                _Context.Add(nav);
            }
            await _Context.SaveChangesAsync();
            return Ok();
        }
        #endregion

    }
}