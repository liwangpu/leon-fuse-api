using ApiModel;
using ApiModel.Entities;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<PagedData<IData>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            var result = await _materialStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, search);
            return MaterialStore.PageQueryDTOTransfer(result);

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
            var accid = AuthMan.GetAccountId(this);
            var exist = await _materialStore.Exist(id);
            if (!exist)
                return NotFound();
            var canRead = await _materialStore.CanRead(accid, id);
            if (!canRead)
                return Forbid();
            var data = await _materialStore.GetByIdAsync(id);
            return Ok(data.ToDTO());
        }
        #endregion

        #region Post 信息材质信息
        /// <summary>
        /// 信息材质信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MaterialCreateModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var material = new Material();
            material.Name = value.Name;
            material.Description = value.Description;
            material.FileAssetId = value.FileAssetId;
            material.CategoryId = value.CategoryId;
            await _materialStore.SatisfyCreate(accid, material, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);

            await _materialStore.CreateAsync(accid, material);
            return Ok(material.ToDTO());
        }
        #endregion

        #region Put 更新材质信息
        /// <summary>
        /// 更新材质信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MaterialEditModel value)
        {
            var exist = await _materialStore.Exist(value.Id);
            if (!exist)
                return NotFound();
            var accid = AuthMan.GetAccountId(this);
            var permission = await _materialStore.CanUpdate(accid, value.Id);
            if (!permission)
                return Forbid();
            var material = await _materialStore.GetByIdAsync(value.Id);
            material.Name = value.Name;
            material.Description = value.Description;
            material.FileAssetId = value.FileAssetId;
            material.CategoryId = value.CategoryId;
            await _materialStore.SatisfyUpdate(accid, material, ModelState);
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            await _materialStore.UpdateAsync(accid, material);
            return Ok(material.ToDTO());
        }
        #endregion

        #region Delete 删除材质信息
        /// <summary>
        /// 删除材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var exist = await _materialStore.Exist(id);
            if (!exist)
                return NotFound();

            var canDelete = await _materialStore.CanDelete(accid, id);
            if (!canDelete)
                return Forbid();
            await _materialStore.DeleteAsync(accid, id);
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
        [ProducesResponseType(typeof(MaterialCreateModel), 200)]
        public IActionResult NewOne()
        {
            return Ok(new MaterialEditModel());
        }
        #endregion

    }
}