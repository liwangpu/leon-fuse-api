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
using System.Threading.Tasks;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class StaticMeshController : ListableController<StaticMesh, StaticMeshDTO>
    {
        #region 构造函数
        public StaticMeshController(ApiDbContext context)
        : base(new StaticMeshStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取模型概要信息
        /// <summary>
        /// 根据分页查询信息获取模型概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<StaticMeshDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model, null);
        }
        #endregion

        #region Get 根据id获取模型信息
        /// <summary>
        /// 根据id获取模型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 创建模型信息
        /// <summary>
        /// 创建模型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        public async Task<IActionResult> Post([FromBody]StaticMeshCreateModel model)
        {
            var mapping = new Func<StaticMesh, Task<StaticMesh>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.Icon = model.IconAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                entity.PackageName = model.PackageName;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新模型信息
        /// <summary>
        /// 更新模型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        public async Task<IActionResult> Put([FromBody]StaticMeshEditModel model)
        {
            var mapping = new Func<StaticMesh, Task<StaticMesh>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.Icon = model.IconAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                entity.PackageName = model.PackageName;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion
    }
}