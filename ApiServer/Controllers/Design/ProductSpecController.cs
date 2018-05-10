using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductSpecController : ListableController<ProductSpec>
    {
        #region 构造函数
        public ProductSpecController(ApiDbContext context)
        : base(new ProductSpecStore(context))
        { }
        #endregion

        #region Get 根据id获取产品规格信息
        /// <summary>
        /// 根据id获取产品规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建产品规格信息
        /// <summary>
        /// 新建产品规格信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductSpecCreateModel model)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ProductId = model.ProductId;
                entity.Price = model.Price;
                return Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 编辑产品规格信息
        /// <summary>
        /// 编辑产品规格信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductSpecEditModel model)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

    }
}