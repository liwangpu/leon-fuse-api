using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ProductStore _ProductStore;

        #region 构造函数
        public ProductsController(ApiDbContext context)
        {
            _ProductStore = new ProductStore(context);
        }
        #endregion

        #region Get 根据分页查询信息获取产品概要信息
        /// <summary>
        /// 根据分页查询信息获取产品概要信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 400)]
        public async Task<PagedData<ProductDTO>> Get(int page, int pageSize, string orderBy, bool desc, string search = "")
        {
            var accid = AuthMan.GetAccountId(this);
            return await _ProductStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
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
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductStore.CanRead(accid, id);
            if (!string.IsNullOrWhiteSpace(msg))
                return NotFound(msg);
            var data = await _ProductStore.GetByIdAsync(accid, id);
            return Ok(data);
        }
        #endregion

        #region Post 新建产品信息
        /// <summary>
        /// 新建产品信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody]ProductEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var product = new Product();
            product.Name = value.Name;
            product.Description = value.Description;
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductStore.CanCreate(accid, product);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            await _ProductStore.SaveOrUpdateAsync(accid, product);
            return Ok(product);
        }
        #endregion

        #region Put 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody]ProductEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var product = await _ProductStore._GetByIdAsync(value.Id);
            if (product == null)
                return BadRequest(ValidityMessage.V_NotDataOrPermissionMsg);
            product.Name = value.Name;
            product.CategoryId = value.CategoryId;
            product.FolderId = value.FolderId;
            product.Description = value.Description;
            product.ModifiedTime = DateTime.Now;
            var msg = await _ProductStore.CanUpdate(accid, product);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            await _ProductStore.SaveOrUpdateAsync(accid, product);
            return Ok(product);
        }
        #endregion

        #region Delete 删除产品信息
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductStore.CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(msg))
                return NotFound(msg);
            await _ProductStore.DeleteAsync(accid, id);
            return Ok();
        }
        #endregion

        #region ChangeICon 更新图标信息
        /// <summary>
        /// 更新图标信息
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("ChangeICon")]
        [HttpPut]
        [ProducesResponseType(typeof(IconModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductStore._GetByIdAsync(icon.ObjId);
            if (spec != null)
            {
                spec.Icon = icon.AssetId;
                await _ProductStore._SaveOrUpdateAsync(spec);
                return Ok(spec);
            }
            return NotFound();
        }
        #endregion
    }
}
