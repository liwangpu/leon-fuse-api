﻿using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Design
{
    /// <summary>
    /// 解决方案控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class SolutionController : ListableController<Solution>
    {
        #region 构造函数
        public SolutionController(ApiDbContext context)
        : base(new SolutionStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取解决方案概要信息
        /// <summary>
        /// 根据分页查询信息获取解决方案概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<SolutionDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion

        #region Get 根据id获取解决方案信息
        /// <summary>
        /// 根据id获取解决方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SolutionDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建解决方案信息
        /// <summary>
        /// 新建解决方案信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(Solution), 200)]
        public async Task<IActionResult> Post([FromBody]SolutionCreateModel model)
        {
            var mapping = new Func<Solution, Task<Solution>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新解决方案信息
        /// <summary>
        /// 更新解决方案信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(Solution), 200)]
        public async Task<IActionResult> Put([FromBody]SolutionEditModel model)
        {
            var mapping = new Func<Solution, Task<Solution>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion
    }
}
