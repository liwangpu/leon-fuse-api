using ApiModel.Entities;
using ApiServer.Models;
using ApiServer.Services;
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
    public class MaterialController : Controller
    {
        private readonly MaterialStore _materialStore;

        #region 构造函数
        public MaterialController(Data.ApiDbContext context)
        {
            _materialStore = new MaterialStore(context);
        }
        #endregion

        #region Get 根据分页查询信息获取材质概要信息
        /// <summary>
        /// 根据分页查询信息获取材质概要信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MaterialDTO>), 200)]
        [ProducesResponseType(typeof(PagedData<MaterialDTO>), 400)]
        public async Task<PagedData<MaterialDTO>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            if (string.IsNullOrEmpty(search))
                return await _materialStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc);
            else
                return await _materialStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
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
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var valid = await _materialStore.CanRead(accid, id);
            if (!string.IsNullOrWhiteSpace(valid))
                return BadRequest(valid);
            var dto = await _materialStore.GetByIdAsync(accid, id);
            return Ok(dto);
        }
        #endregion

        #region Post 信息材质信息
        /// <summary>
        /// 信息材质信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody]MaterialEditModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var material = new Material();
            material.Name = value.Name;
            material.Description = value.Description;
            material.FileAssetId = value.FileAssetId;
            material.CategoryId = value.CategoryId;
            var msg = await _materialStore.CanCreate(accid, material);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);

            var dto = await _materialStore.CreateAsync(accid, material);
            return Ok(dto);
        }
        #endregion

        #region Put 更新材质信息
        /// <summary>
        /// 更新材质信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody]MaterialEditModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var material = await _materialStore._GetByIdAsync(value.Id);
            if (material == null)
                return BadRequest(ValidityMessage.V_NotDataOrPermissionMsg);
            material.Name = value.Name;
            material.Description = value.Description;
            material.FileAssetId = value.FileAssetId;
            material.CategoryId = value.CategoryId;
            var msg = await _materialStore.CanUpdate(accid, material);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            var dto = await _materialStore.UpdateAsync(accid, material);
            return Ok(dto);
        }
        #endregion

        #region Delete 删除材质信息
        /// <summary>
        /// 删除材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _materialStore.CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            var material = await _materialStore._GetByIdAsync(id);
            await _materialStore.DeleteAsync(accid, material);
            return Ok();
        }
        #endregion

        #region NewOne Post,Put示例数据
        /// <summary>
        /// Post,Put示例数据
        /// </summary>
        /// <returns></returns>
        [Route("NewOne")]
        [HttpGet]
        [ProducesResponseType(typeof(MaterialEditModel), 200)]
        public IActionResult NewOne()
        {
            return Ok(new MaterialEditModel());
        } 
        #endregion

    }
}