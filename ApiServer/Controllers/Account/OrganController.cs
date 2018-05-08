using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class OrganController : Controller
    {
        private readonly OrganizationStore _OrganizationStore;

        #region 构造函数
        public OrganController(ApiDbContext context)
        {
            _OrganizationStore = new OrganizationStore(context);
        }
        #endregion

        #region Get 根据分页查询信息获取组织概要信息
        /// <summary>
        /// 根据分页查询信息获取组织概要信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrganizationDTO>), 200)]
        [ProducesResponseType(typeof(PagedData<OrganizationDTO>), 400)]
        public async Task<PagedData<OrganizationDTO>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            var accid = AuthMan.GetAccountId(this);
            if (string.IsNullOrEmpty(search))
                return await _OrganizationStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc);
            else
                return await _OrganizationStore.SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }
        #endregion

        #region Get 根据Id获取组织信息
        /// <summary>
        /// 根据Id获取组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var valid = await _OrganizationStore.CanRead(accid, id);
            if (!string.IsNullOrWhiteSpace(valid))
                return BadRequest(valid);
            var dto = await _OrganizationStore.GetByIdAsync(accid, id);
            return Ok(dto);
        }
        #endregion

        #region Post 新建组织信息
        /// <summary>
        /// 新建组织信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Organization), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody]OrganizationEditModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var organ = new Organization();
            organ.Name = value.Name;
            organ.ParentId = value.ParentId;
            organ.Description = value.Description;
            organ.Mail = !string.IsNullOrWhiteSpace(value.Mail) ? value.Mail.Trim() : string.Empty;
            organ.Location = value.Location;
            organ.OwnerId = value.OwnerId;
            organ.Creator = accid;
            organ.Modifier = accid;
            var msg = await _OrganizationStore.CanCreate(accid, organ);
            if (!string.IsNullOrWhiteSpace(msg))
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
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody]OrganizationEditModel value)
        {
            var accid = AuthMan.GetAccountId(this);
            var organ = await _OrganizationStore._GetByIdAsync(value.Id);
            if (organ == null)
                return BadRequest(ValidityMessage.V_NotDataOrPermissionMsg);
            organ.Name = value.Name;
            organ.Description = value.Description;
            organ.Mail = value.Mail;
            organ.Location = value.Location;
            organ.ModifiedTime = DateTime.Now;
            organ.Modifier = accid;

            var msg = await _OrganizationStore.CanUpdate(accid, organ);
            if (!string.IsNullOrWhiteSpace(msg))
                return BadRequest(msg);
            var dto = await _OrganizationStore.UpdateAsync(accid, organ);
            return Ok(dto);
        }
        #endregion

        #region GetOwner 获取组织管理员信息
        /// <summary>
        /// 获取组织管理员信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Owner")]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetOwner(string organId)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var valid = await _OrganizationStore.CanRead(accid, organId);
            if (!string.IsNullOrWhiteSpace(valid))
                return BadRequest(valid);
            var dto = await _OrganizationStore.GetOrganOwner(organId);
            return Ok(dto);
        }
        #endregion


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
