//using ApiModel.Entities;
//using ApiServer.Controllers.Common;
//using ApiServer.Data;
//using ApiServer.Filters;
//using ApiServer.Models;
//using ApiServer.Services;
//using ApiServer.Stores;
//using BambooCore;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace ApiServer.Controllers.Design
//{
//    [Authorize]
//    [Route("/[controller]")]
//    public class ProductsController : ListableController
//    {
//        private readonly ProductStore _ProductStore;

//        #region 构造函数
//        public ProductsController(ApiDbContext context)
//        {
//            _ProductStore = new ProductStore(context);
//        }
//        #endregion

//        #region Get 根据分页查询信息获取产品概要信息
//        /// <summary>
//        /// 根据分页查询信息获取产品概要信息
//        /// </summary>
//        /// <param name="page"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="orderBy"></param>
//        /// <param name="desc"></param>
//        /// <param name="search"></param>
//        /// <param name="categoryId">分类Id</param>
//        /// <returns></returns>
//        [HttpGet]
//        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
//        [ProducesResponseType(typeof(PagedData<ProductDTO>), 400)]
//        public async Task<PagedData<ProductDTO>> Get(int page, int pageSize, string orderBy, bool desc, string search = "", string categoryId = "")
//        {
//            var accid = AuthMan.GetAccountId(this);
//            if (string.IsNullOrEmpty(search))
//                return await _ProductStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, categoryId);
//            else
//                return await _ProductStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, categoryId, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
//        }
//        #endregion

//        #region Get 根据id获取产品信息
//        /// <summary>
//        /// 根据id获取产品信息
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ProductDTO), 200)]
//        public async Task<IActionResult> Get(string id)
//        {
//            var accid = AuthMan.GetAccountId(this);
//            var exist = await _ProductStore.Exist(id);
//            if (!exist)
//                return NotFound();
//            var canRead = await _ProductStore.CanRead(accid, id);
//            if (!canRead)
//                return Forbid();
//            var dto = await _ProductStore.GetByIdAsync(accid, id);
//            return Ok(dto);
//        }
//        #endregion

//        #region Post 新建产品信息
//        /// <summary>
//        /// 新建产品信息
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ValidateModel]
//        [ProducesResponseType(typeof(Product), 200)]
//        [ProducesResponseType(typeof(ValidationResultModel), 400)]
//        public async Task<IActionResult> Post([FromBody]ProductCreateModel value)
//        {
//            var accid = AuthMan.GetAccountId(this);
//            var product = new Product();
//            product.Name = value.Name;
//            product.Description = value.Description;
//            await _ProductStore.CanCreate(accid, product, ModelState);
//            if (!ModelState.IsValid)
//                return new ValidationFailedResult(ModelState);

//            var dto = await _ProductStore.CreateAsync(accid, product);
//            return Ok(dto);
//        }
//        #endregion

//        #region Put 更新产品信息
//        /// <summary>
//        /// 更新产品信息
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPut]
//        [ValidateModel]
//        [ProducesResponseType(typeof(Product), 200)]
//        [ProducesResponseType(typeof(ValidationResultModel), 400)]
//        public async Task<IActionResult> Put([FromBody]ProductEditModel value)
//        {
//            var exist = await _ProductStore.Exist(value.Id);
//            if (!exist)
//                return NotFound();

//            var accid = AuthMan.GetAccountId(this);
//            var product = await _ProductStore.GetByIdAsync(value.Id);
//            product.Name = value.Name;
//            product.CategoryId = value.CategoryId;
//            product.FolderId = value.FolderId;
//            product.Description = value.Description;
//            product.ModifiedTime = DateTime.Now;
//            await _ProductStore.CanUpdate(accid, product, ModelState);
//            if (!ModelState.IsValid)
//                return new ValidationFailedResult(ModelState);

//            var dto = await _ProductStore.UpdateAsync(accid, product);
//            return Ok(dto);
//        }
//        #endregion

//        #region Delete 删除产品信息
//        /// <summary>
//        /// 删除产品信息
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var exist = await _ProductStore.Exist(id);
//            if (!exist)
//                return NotFound();

//            var accid = AuthMan.GetAccountId(this);
//            var canDelete = await _ProductStore.CanDelete(accid, id);
//            if (!canDelete)
//                return Forbid();
//            await _ProductStore.DeleteAsync(accid, id);
//            return Ok();
//        }
//        #endregion

//        #region ChangeICon 更新图标信息
//        /// <summary>
//        /// 更新图标信息
//        /// </summary>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        [Route("ChangeICon")]
//        [HttpPut]
//        [ProducesResponseType(typeof(IconModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductStore.GetByIdAsync(icon.ObjId);
//            if (spec != null)
//            {
//                spec.Icon = icon.AssetId;
//                await _ProductStore._SaveOrUpdateAsync(spec);
//                return Ok(spec);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region NewOne Post,Put示例数据
//        /// <summary>
//        /// Post,Put示例数据
//        /// </summary>
//        /// <returns></returns>
//        [Route("NewOne")]
//        [HttpGet]
//        [ProducesResponseType(typeof(ProductCreateModel), 200)]
//        public IActionResult NewOne()
//        {
//            return Ok(new ProductCreateModel());
//        }
//        #endregion
//    }
//}
