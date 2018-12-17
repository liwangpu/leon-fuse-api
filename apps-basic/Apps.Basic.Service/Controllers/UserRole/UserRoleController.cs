using Apps.Base.Common.Attributes;
using Apps.Base.Common.Consts;
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
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 用户角色控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserRoleController : ListviewController<UserRole>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public UserRoleController(IRepository<UserRole> repository, AppDbContext context)
         : base(repository)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取用户角色信息
        /// <summary>
        /// 根据分页获取用户角色信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="excludeInner"></param>
        /// <param name="innerOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<UserRoleDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, bool excludeInner = false, bool innerOnly = false)
        {
            var toDTO = new Func<UserRole, Task<UserRoleDTO>>(async (entity) =>
            {
                var dto = new UserRoleDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.IsInner = entity.IsInner;
                return await Task.FromResult(dto);
            });

            var advanceQuery = new Func<IQueryable<UserRole>, Task<IQueryable<UserRole>>>(async (query) =>
            {
                if (excludeInner)
                {
                    query = query.Where(x => x.IsInner == false && x.OrganizationId == CurrentAccountOrganizationId);
                }


                if (innerOnly)
                    query = query.Where(x => x.IsInner == true);
                return await Task.FromResult(query);
            });
            return await _PagingRequest(model, toDTO, advanceQuery);
        }
        #endregion

        #region Get 根据Id获取用户角色信息
        /// <summary>
        /// 根据Id获取用户角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserRoleDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<UserRole, Task<UserRoleDTO>>(async (entity) =>
            {
                var dto = new UserRoleDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.IsInner = entity.IsInner;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建用户角色
        /// <summary>
        /// 创建用户角色
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(UserRoleDTO), 200)]
        public async Task<IActionResult> Post([FromBody]UserRoleCreateModel model)
        {
            var mapping = new Func<UserRole, Task<UserRole>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.OrganizationId = CurrentAccountOrganizationId;
                entity.ActiveFlag = AppConst.Active;
                var curAccountRole = await _Context.Accounts.Where(x => x.Id == CurrentAccountId).Select(x => x.InnerRoleId).FirstOrDefaultAsync();
                if (curAccountRole == UserRoleConst.SysAdmin)
                {
                    entity.IsInner = true;
                }
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更改用户角色信息
        /// <summary>
        /// 更改用户角色信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(UserRoleDTO), 200)]
        public async Task<IActionResult> Put([FromBody]UserRoleEditModel model)
        {
            var mapping = new Func<UserRole, Task<UserRole>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion
    }
}