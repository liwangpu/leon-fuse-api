using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly Repository<Department> repo;
        private readonly DepartmentStore _DepartmentStore;
        public DepartmentController(ApiDbContext context)
        {
            repo = new Repository<Department>(context);
            _DepartmentStore = new DepartmentStore(context);
        }

        [HttpGet("{id}")]
        [Produces(typeof(Department))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            await repo.Context.Entry(res).Collection(d => d.Members).LoadAsync();
            return Ok(res);//return Forbid();
        }

        #region Post 新建部门信息
        /// <summary>
        /// 新建部门信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post([FromBody]DepartmentEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var department = new Department();
            department.Name = value.Name;
            department.ParentId = value.ParentId;
            department.Description = value.Description;
            department.ModifiedTime = DateTime.Now;
            department.Creator = accid;
            department.OrganizationId = value.OrganizationId;

            var msg = await _DepartmentStore.CanCreate(accid, department);
            if (msg.Count > 0)
                return BadRequest(msg);

            var dto = await _DepartmentStore.CreateAsync(accid, department);
            return Ok(dto);
        }
        #endregion

        #region Put 编辑部门信息
        /// <summary>
        /// 编辑部门信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(DepartmentDTO), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Put([FromBody]DepartmentEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var department = await _DepartmentStore._GetByIdAsync(value.Id);
            department.Name = value.Name;
            department.ParentId = value.ParentId;
            department.Description = value.Description;
            department.ModifiedTime = DateTime.Now;
            department.Modifier = accid;

            if (department == null)
                return BadRequest(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
            var msg = await _DepartmentStore.CanUpdate(accid, department);
            if (msg.Count > 0)
                return BadRequest(msg);
            var dto = await _DepartmentStore.UpdateAsync(accid, department);
            return Ok(dto);
        } 
        #endregion

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
            if (bOk)
                return Ok();
            return NotFound();//return Forbid();
        }


        [Route("ByOrgan")]
        [HttpGet]
        [Produces(typeof(Department))]
        public async Task<IActionResult> GetByOrgan(string organId)
        {
            var res = await repo.GetDataSet(AuthMan.GetAccountId(this)).Where(x => x.OrganizationId == organId).ToListAsync();
            if (res == null)
                return NotFound();
            return Ok(res);
        }


    }
}