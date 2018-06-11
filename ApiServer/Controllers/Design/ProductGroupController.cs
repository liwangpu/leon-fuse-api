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
    public class ProductGroupController : ListableController<ProductGroup, ProductGroupDTO>
    {
        #region 构造函数
        public ProductGroupController(ApiDbContext context)
        : base(new ProductGroupStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取区域类型概要信息
        /// <summary>
        /// 根据分页查询信息获取区域类型概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductGroupDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model, null);
        }
        #endregion

        #region Get 根据id获取区域类型信息
        /// <summary>
        /// 根据id获取区域类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建区域类型信息
        /// <summary>
        /// 新建区域类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductGroupCreateModel model)
        {
            var mapping = new Func<ProductGroup, Task<ProductGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Items = model.Items;
                entity.PivotLocation = model.PivotLocation;
                entity.PivotType = model.PivotType;
                entity.Orientation = model.Orientation;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新区域类型信息
        /// <summary>
        /// 更新区域类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductGroupEditModel model)
        {
            var mapping = new Func<ProductGroup, Task<ProductGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Items = model.Items;
                entity.PivotLocation = model.PivotLocation;
                entity.PivotType = model.PivotType;
                entity.Orientation = model.Orientation;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion
    }
}