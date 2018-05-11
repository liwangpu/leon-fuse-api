using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class MaterialController : ListableController<Material>
    {
        #region 构造函数
        public MaterialController(ApiDbContext context)
        : base(new MaterialStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取材质概要信息
        /// <summary>
        /// 根据分页查询信息获取材质概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MaterialDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion

        #region Get 根据id获取材质信息
        /// <summary>
        /// 根据id获取材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 信息材质信息
        /// <summary>
        /// 信息材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MaterialCreateModel model)
        {
            var mapping = new Func<Material, Task<Material>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.CategoryId = model.CategoryId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新材质信息
        /// <summary>
        /// 更新材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MaterialEditModel model)
        {
            var mapping = new Func<Material, Task<Material>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.CategoryId = model.CategoryId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

    }
}