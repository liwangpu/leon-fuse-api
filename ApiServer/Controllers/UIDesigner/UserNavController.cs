using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers.UIDesigner
{
    [Authorize]
    [Route("/[controller]")]
    public class UserNavController : ListableController<UserNav, UserNavDTO>
    {
        #region 构造函数
        public UserNavController(IRepository<UserNav, UserNavDTO> repository)
         : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取导航概要信息
        /// <summary>
        /// 根据分页查询信息获取导航概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<UserNavDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion


    }
}