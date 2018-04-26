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
    public class SolutionController : Controller
    {
        private readonly SolutionStore _SolutionStore;

        #region 构造函数
        public SolutionController(ApiDbContext context)
        {   
            _SolutionStore = new SolutionStore(context);
        }
        #endregion

        #region Get 根据分页查询信息获取解决方案概要信息
        /// <summary>
        /// 根据分页查询信息获取解决方案概要信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData1<SolutionDTO>), 200)]
        [ProducesResponseType(typeof(PagedData1<SolutionDTO>), 400)]
        public async Task<PagedData1<SolutionDTO>> Get(int page, int pageSize, string orderBy, bool desc, string search = "")
        {
            var accid = AuthMan.GetAccountId(this);
            return await _SolutionStore.SimpleQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }
        #endregion

        #region Get 根据id获取解决方案信息
        /// <summary>
        /// 根据id获取解决方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SolutionDTO), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _SolutionStore.CanRead(accid, id);
            if (msg.Count > 0)
                return NotFound(msg);
            var data = await _SolutionStore.GetByIdAsync(accid, id);
            return Ok(data);
        }
        #endregion

        #region Post 新建解决方案信息
        /// <summary>
        /// 新建解决方案信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Solution), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post([FromBody]SolutionEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var solution = new Solution();
            solution.Name = value.Name;
            solution.Description = value.Description;
            var accid = AuthMan.GetAccountId(this);
            var msg = await _SolutionStore.CanCreate(accid, solution);
            if (msg.Count > 0)
                return BadRequest(msg);
            await _SolutionStore.SaveOrUpdateAsync(accid, solution);
            return Ok(solution);
        }
        #endregion

        #region Put 更新解决方案信息
        /// <summary>
        /// 更新解决方案信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Solution), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Put([FromBody]SolutionEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var solution = await _SolutionStore._GetByIdAsync(value.Id);
            if (solution == null)
                return BadRequest(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
            solution.Name = value.Name;
            solution.Name = value.Name;
            solution.Description = value.Description;
            solution.ModifiedTime = new DateTime();
            var msg = await _SolutionStore.CanUpdate(accid, solution);
            if (msg.Count > 0)
                return BadRequest(msg);
            await _SolutionStore.SaveOrUpdateAsync(accid, solution);
            return Ok(solution);
        }
        #endregion

        #region Delete 删除解决方案信息
        /// <summary>
        /// 删除解决方案信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _SolutionStore.CanDelete(accid, id);
            if (msg.Count > 0)
                return NotFound(msg);
            await _SolutionStore.DeleteAsync(accid, id);
            return Ok();
        }
        #endregion

    }
}
