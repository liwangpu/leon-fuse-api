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

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class StaticMeshController : Controller
    {
        private readonly ApiDbContext _ApiContext;
        private readonly Repository<StaticMesh> repo;
        private readonly StaticMeshStore _StaticMeshStore;
        public StaticMeshController(ApiDbContext context)
        {
            repo = new Repository<StaticMesh>(context);
            _ApiContext = context;
            _StaticMeshStore = new StaticMeshStore(context);
        }

        #region Get 根据分页查询信息获取模型概要信息
        /// <summary>
        /// 根据分页查询信息获取模型概要信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<StaticMeshDTO>), 200)]
        [ProducesResponseType(typeof(PagedData<StaticMeshDTO>), 400)]
        public async Task<PagedData<StaticMeshDTO>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            return await _StaticMeshStore.SimpleQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }
        #endregion

        #region Get 根据id获取模型信息
        /// <summary>
        /// 根据id获取模型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _StaticMeshStore.CanRead(accid, id);
            if (!string.IsNullOrWhiteSpace(msg))
                return NotFound(msg);
            var data = await _StaticMeshStore.GetByIdAsync(id);
            return Ok(data);
        }
        #endregion

        #region Post 创建模型信息
        /// <summary>
        /// 创建模型信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody]StaticMeshEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var entity = new StaticMesh();
            entity.Name = value.Name;
            entity.Description = value.Description;
            entity.FileAssetId = value.FileAssetId;
            entity.Icon = value.Icon;
            var accid = AuthMan.GetAccountId(this);
            var msg = await _StaticMeshStore.CanCreate(accid, entity);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            await _StaticMeshStore.SaveOrUpdateAsync(accid, entity);
            return Ok(entity.ToDTO());
        }
        #endregion

        #region Put 更新模型信息
        /// <summary>
        /// 更新模型信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(StaticMeshDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody]StaticMeshEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var mesh = await _StaticMeshStore._GetByIdAsync(value.Id);
            if (mesh == null)
                return BadRequest(ValidityMessage.V_NotDataOrPermissionMsg );
            mesh.Name = value.Name;
            mesh.Name = value.Name;
            mesh.Description = value.Description;
            mesh.ModifiedTime = new DateTime();
            var msg = await _StaticMeshStore.CanUpdate(accid, mesh);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            await _StaticMeshStore.SaveOrUpdateAsync(accid, mesh);
            return Ok(mesh.ToDTO());
        }
        #endregion

        #region Delete 删除模型信息
        /// <summary>
        /// 删除模型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _StaticMeshStore.CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(msg))
                return NotFound(msg);
            await _StaticMeshStore.DeleteAsync(accid, id);
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
        [ProducesResponseType(typeof(StaticMeshEditModel), 200)]
        public IActionResult NewOne()
        {
            return Ok(new StaticMeshEditModel());
        }
        #endregion
    }
}