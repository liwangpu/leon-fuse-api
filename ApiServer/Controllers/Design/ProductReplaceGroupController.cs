using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductReplaceGroupController : ListableController<ProductReplaceGroup, ProductReplaceGroupDTO>
    {
        #region 构造函数
        public ProductReplaceGroupController(IRepository<ProductReplaceGroup, ProductReplaceGroupDTO> repository)
        : base(repository)
        {

        }
        #endregion

        #region Get 根据分页查询信息获取产品替换组概要信息
        /// <summary>
        /// 根据分页查询信息获取产品替换组概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductReplaceGroupDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion

        #region Get 根据id获取产品替换组信息
        /// <summary>
        /// 根据id获取产品替换组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductReplaceGroupDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建产品替换组信息
        /// <summary>
        /// 新建产品替换组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductReplaceGroupCreateModel model)
        {
            var mapping = new Func<ProductReplaceGroup, Task<ProductReplaceGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.DefaultItemId = model.DefaultItemId;
                entity.GroupItemIds = model.ItemIds;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新产品替换组信息
        /// <summary>
        /// 更新产品替换组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductReplaceGroupEditModel model)
        {
            var mapping = new Func<ProductReplaceGroup, Task<ProductReplaceGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.DefaultItemId = model.DefaultItemId;
                entity.GroupItemIds = model.ItemIds;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

    }
}