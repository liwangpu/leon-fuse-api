using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Models;
using ApiServer.Repositories;
using BambooCore;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("/[controller]")]
    public class WorkFlowController : ListableController<WorkFlow, WorkFlowDTO>
    {
        #region 构造函数
        public WorkFlowController(IRepository<WorkFlow, WorkFlowDTO> repository)
        : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取用户角色概要信息
        /// <summary>
        /// 根据分页查询信息获取用户角色概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<WorkFlowDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion
    }
}