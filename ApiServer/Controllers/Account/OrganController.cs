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


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class OrganController : Controller
    {
        private readonly Repository<Organization> repo;
        private readonly OrganizationStore _OrganizationStore;


        public OrganController(ApiDbContext context)
        {
            repo = new Repository<Organization>(context);
            _OrganizationStore = new OrganizationStore(context);
        }

        [HttpGet]
        public async Task<PagedData<Organization>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
               d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(Organization))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            await repo.Context.Entry(res).Reference(d => d.Owner).LoadAsync();
            await repo.Context.Entry(res).Collection(d => d.Departments).LoadAsync();
            return Ok(res);//return Forbid();
        }

        #region Post 新建组织信息
        /// <summary>
        /// 新建组织信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post([FromBody]OrganizationEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var organ = new Organization();
            organ.Name = value.Name;
            organ.ParentId = value.ParentId;
            organ.Description = value.Description;
            organ.Mail = value.Mail;
            organ.Location = value.Location;
            organ.OwnerId = value.OwnerId;
            organ.Creator = accid;
            organ.Modifier = accid;
            var msg = await _OrganizationStore.CanCreate(accid, organ);
            if (msg.Count > 0)
                return BadRequest(msg);

            var dto = await _OrganizationStore.CreateAsync(accid, organ);
            return Ok(dto);
        }
        #endregion

        #region Put 修改组织信息
        /// <summary>
        /// 修改组织信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Put([FromBody]OrganizationEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var organ = await _OrganizationStore._GetByIdAsync(value.Id);
            organ.Name = value.Name;
            organ.Description = value.Description;
            organ.Mail = value.Mail;
            organ.Location = value.Location;
            organ.ModifiedTime = DateTime.Now;
            organ.Modifier = accid;
            if (organ == null)
                return BadRequest(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
            var msg = await _OrganizationStore.CanUpdate(accid, organ);
            if (msg.Count > 0)
                return BadRequest(msg);
            var dto = await _OrganizationStore.UpdateAsync(accid, organ);
            return Ok(dto);
        } 
        #endregion

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
