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
    public class ProductsController : ListableController<Product>
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
            return await _GetPagingRequest(model);
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
            return await _GetByIdRequest(id);
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
            var mapping = new Func<Product, Task<Product>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return Task.FromResult(entity);
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
            var mapping = new Func<Product, Task<Product>>((entity) =>
            {
                entity.Name = model.Name;
                entity.CategoryId = model.CategoryId;
                entity.FolderId = model.FolderId;
                entity.Description = model.Description;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region ChangeICon 更新图标信息
        ///// <summary>
        ///// 更新图标信息
        ///// </summary>
        ///// <param name="icon"></param>
        ///// <returns></returns>
        //[Route("ChangeICon")]
        //[HttpPut]
        //[ProducesResponseType(typeof(IconModel), 200)]
        //[ProducesResponseType(typeof(List<string>), 404)]
        //public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        //{
        //    if (ModelState.IsValid == false)
        //        return BadRequest(ModelState);

        //    var accid = AuthMan.GetAccountId(this);
        //    var spec = await _ProductStore.GetByIdAsync(icon.ObjId);
        //    if (spec != null)
        //    {
        //        spec.Icon = icon.AssetId;
        //        await _ProductStore._SaveOrUpdateAsync(spec);
        //        return Ok(spec);
        //    }
        //    return NotFound();
        //}
        #endregion


    }
}