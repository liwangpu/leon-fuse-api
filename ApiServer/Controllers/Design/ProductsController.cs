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
    public class ProductsController : ListableController<Product, ProductDTO>
    {
        #region 构造函数
        public ProductsController(ApiDbContext context)
        : base(new ProductStore(context))
        { }
        #endregion

        #region Get 根据分页查询信息获取产品概要信息
        /// <summary>
        /// 根据分页查询信息获取产品概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "")
        {
            var qMapping = new Action<List<string>>((query) =>
            {
                if (!string.IsNullOrWhiteSpace(categoryId))
                    query.Add(string.Format("CategoryId={0}", categoryId));

                //query.AddRange(KeyWordSearchQ(model.Search));
            });
            return await _GetPagingRequest(model, qMapping, ResourceTypeEnum.Organizational);
        }
        #endregion

        #region Get 根据id获取产品信息
        /// <summary>
        /// 根据id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id, ResourceTypeEnum.Organizational);
        }
        #endregion

        #region Post 新建产品信息
        /// <summary>
        /// 新建产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductCreateModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductEditModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

    }
}