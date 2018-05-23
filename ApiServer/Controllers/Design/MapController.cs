using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class MapController : ListableController<Map, MapDTO>
    {
        #region 构造函数
        public MapController(ApiDbContext context)
        : base(new MapStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取地图概要信息
        /// <summary>
        /// 根据分页查询信息获取地图概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MapDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var qMapping = new Action<List<string>>((query) =>
            {

            });
            return await _GetPagingRequest(model, qMapping);
        }
        #endregion

        #region Get 根据id获取地图信息
        /// <summary>
        /// 根据id获取地图信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MapDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建地图信息
        /// <summary>
        /// 新建地图信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MapCreateModel model)
        {
            var mapping = new Func<Map, Task<Map>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新地图信息
        /// <summary>
        /// 更新地图信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MapEditModel model)
        {
            var mapping = new Func<Map, Task<Map>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

    }
}