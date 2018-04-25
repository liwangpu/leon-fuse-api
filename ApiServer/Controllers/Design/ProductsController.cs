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
        [ProducesResponseType(typeof(PagedData1<ProductDTO>), 200)]
        [ProducesResponseType(typeof(PagedData1<ProductDTO>), 400)]
        public async Task<PagedData1<ProductDTO>> Get(int page, int pageSize, string orderBy, bool desc, string search = "")
        {
            var accid = AuthMan.GetAccountId(this);
            return await _ProductStore.SimpleQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
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
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var res = await _ProductStore._GetByIdAsync(accid, id);
            if (res == null)
                return NotFound(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
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
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post([FromBody]ProductEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var product = new Product();
            product.Name = value.Name;
            product.Description = value.Description;
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductStore.SaveOrUpdateAsync(accid, product);
            if (msg.Count > 0)
                return BadRequest(msg);
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
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Put([FromBody]ProductEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var product = await _ProductStore._GetByIdAsync(accid, value.Id);
            if (product == null)
                return BadRequest(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
            product.Name = value.Name;
            product.Description = value.Description;
            product.ModifiedTime = new DateTime();
            var msg = await _ProductStore.SaveOrUpdateAsync(accid, product);
            if (msg.Count > 0)
                return BadRequest(msg);
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
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductStore.DeleteAsync(accid, id);
            if (msg.Count > 0)
                return NotFound(msg);
            return Ok();
        }
        #endregion

    }
}
