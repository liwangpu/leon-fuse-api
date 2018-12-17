using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 组织管理控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrganController : ServiceBaseController<Organization>
    {
        protected readonly AppDbContext _Context;
        protected readonly IRepository<Account> _AccountRepository;
        protected readonly ITreeRepository<OrganizationTree> _OrganTreeRepository;

        #region 构造函数
        public OrganController(IRepository<Organization> repository, IRepository<Account> accountRepository, ITreeRepository<OrganizationTree> organTreeRepository, AppDbContext context)
            : base(repository)
        {
            _Context = context;
            _AccountRepository = accountRepository;
            _OrganTreeRepository = organTreeRepository;
        }
        #endregion

        #region Get 根据分页获取组织信息
        /// <summary>
        /// 根据分页获取组织信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<OrganizationDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var toDTO = new Func<Organization, Task<OrganizationDTO>>(async (entity) =>
            {
                var dto = new OrganizationDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Location = entity.Location;
                dto.ActivationTime = entity.ActivationTime;
                dto.ExpireTime = entity.ExpireTime;
                dto.OrganizationTypeId = entity.OrganizationTypeId;
                if (!string.IsNullOrWhiteSpace(dto.OrganizationTypeId))
                {
                    var organType = await _Context.OrganizationTypes.FirstOrDefaultAsync(x => x.Id == dto.OrganizationTypeId);
                    dto.OrganizationTypeName = organType != null ? organType.Name : string.Empty;
                }
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
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
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<Organization, Task<OrganizationDTO>>(async (entity) =>
            {
                var dto = new OrganizationDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.ActivationTime = entity.ActivationTime;
                dto.ExpireTime = entity.ExpireTime;
                dto.Location = entity.Location;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationTypeId = entity.OrganizationTypeId;
                if (!string.IsNullOrWhiteSpace(dto.OrganizationTypeId))
                {
                    var organType = await _Context.OrganizationTypes.FirstOrDefaultAsync(x => x.Id == dto.OrganizationTypeId);
                    dto.OrganizationTypeName = organType != null ? organType.Name : string.Empty;
                }
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建组织
        /// <summary>
        /// 创建组织
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        public async Task<IActionResult> Post([FromBody]OrganizationCreateModel model)
        {
            var mapping = new Func<Organization, Task<Organization>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.OrganizationTypeId = model.OrganizationTypeId;
                entity.Location = model.Location;
                entity.ActivationTime = model.ActivationTime;
                entity.ExpireTime = model.ExpireTime;
                entity.ActiveFlag = AppConst.Active;
                return await Task.FromResult(entity);
            });
            var afterCreated = new Func<Organization, Task<IActionResult>>(async (organ) =>
            {
                #region 创建默认管理员
                {
                    var admin = new Account();
                    admin.Name = "超级管理员";
                    admin.ActivationTime = organ.ActivationTime;
                    admin.ExpireTime = organ.ExpireTime;
                    admin.Creator = CurrentAccountId;
                    admin.Modifier = CurrentAccountId;
                    admin.CreatedTime = DateTime.Now;
                    admin.ModifiedTime = admin.CreatedTime;
                    admin.ActiveFlag = AppConst.Active;
                    admin.Organization = organ;

                    if (organ.OrganizationTypeId == OrganTyeConst.Brand)
                        admin.InnerRoleId = UserRoleConst.BrandAdmin;
                    else if (organ.OrganizationTypeId == OrganTyeConst.Partner)
                        admin.InnerRoleId = UserRoleConst.PartnerAdmin;
                    else if (organ.OrganizationTypeId == OrganTyeConst.Supplier)
                        admin.InnerRoleId = UserRoleConst.SupplierAdmin;
                    else { }

                    await _AccountRepository.CreateAsync(admin, CurrentAccountId);

                    organ.OwnerId = admin.Id;
                    //更新组织管理员信息
                    _Context.Organizations.Update(organ);
                    await _Context.SaveChangesAsync();
                }
                #endregion

                #region 创建组织管理树
                {
                    var organTree = new OrganizationTree();
                    organTree.ObjId = organ.Id;
                    organTree.NodeType = organ.OrganizationTypeId;
                    if (CurrentAccountOrganizationId == AppConst.BambooOrganId)
                    {
                        organTree.ParentId = string.Empty;
                    }
                    else
                    {
                        var refTree = await _OrganTreeRepository.GetNodeByObjId(CurrentAccountOrganizationId);
                        organTree.ParentId = refTree != null ? refTree.Id : string.Empty;
                    }

                    await _OrganTreeRepository.CreateAsync(organTree, CurrentAccountId);
                }
                #endregion

                return await Get(organ.Id);
            });
            return await _PostRequest(mapping, afterCreated);
        }
        #endregion

        #region Put 更改组织信息
        /// <summary>
        /// 更改组织信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(OrganizationDTO), 200)]
        public async Task<IActionResult> Put([FromBody]OrganizationEditModel model)
        {
            var mapping = new Func<Organization, Task<Organization>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.OrganizationTypeId = model.OrganizationTypeId;
                entity.Location = model.Location;
                entity.ActivationTime = model.ActivationTime;
                entity.ExpireTime = model.ExpireTime;
                return await Task.FromResult(entity);
            });

            var afterCreated = new Func<Organization, Task<IActionResult>>(async (organ) =>
            {

                #region 同步超级管理员的激活时间和过期时间
                var admin = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == organ.OwnerId);
                if (admin != null)
                {
                    admin.ActivationTime = organ.ActivationTime;
                    admin.ExpireTime = organ.ExpireTime;
                    await _AccountRepository.UpdateAsync(admin, CurrentAccountId);
                }
                #endregion

                return await Get(organ.Id);
            });
            return await _PutRequest(model.Id, mapping, afterCreated);
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
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> GetOwner(string organId)
        {
            var organ = await _Context.Organizations.FirstOrDefaultAsync(x => x.Id == organId);
            var dto = new AccountDTO();
            if (organ != null)
                return RedirectToAction("Get", "Account", new { id = organ.OwnerId });
            return NotFound();
        }
        #endregion
    }
}