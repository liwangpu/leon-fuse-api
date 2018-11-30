using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 用户角色控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserRoleController : ListviewController<UserRole>
    {
        #region 构造函数
        public UserRoleController(IRepository<UserRole> repository)
         : base(repository)
        {
        }
        #endregion

        #region Get 根据分页获取用户导航栏信息
        /// <summary>
        /// 根据分页获取用户导航栏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<UserRoleDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _PagingRequest(model);
        }
        #endregion

        public override Task<IActionResult> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}