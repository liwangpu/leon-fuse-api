using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Contexts;
using Apps.Basic.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ServiceBaseController<Account>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public AccountController(IRepository<Account> repository, AppDbContext context)
            : base(repository)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取用户信息
        /// <summary>
        /// 根据分页获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _PagingRequest(model);
        }
        #endregion

        #region Get 根据Id获取用户信息
        /// <summary>
        /// 根据Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<Account, Task<AccountDTO>>(async (entity) =>
            {
                var dto = new AccountDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Phone = entity.Phone;
                dto.Mail = entity.Mail;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建用户
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> Post([FromBody]AccountCreateModel model)
        {
            var mapping = new Func<Account, Task<Account>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Password = model.Password;
                entity.Mail = model.Mail;
                entity.Phone = model.Phone;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    entity.Password = Md5.CalcString(model.Password);
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更改用户信息
        /// <summary>
        /// 更改用户信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> Put([FromBody]AccountEditModel mode)
        {
            var mapping = new Func<Account, Task<Account>>(async (entity) =>
            {
                entity.Name = mode.Name;
                entity.Password = mode.Password;
                entity.Mail = mode.Mail;
                entity.Phone = mode.Phone;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(mode.Id, mapping);
        }
        #endregion

        #region Delete 删除用户信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        public async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region GetProfile 获取当前用户信息
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [Route("Profile")]
        [HttpGet]
        [ProducesResponseType(typeof(AccountProfileDTO), 200)]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = await _Context.Accounts.FindAsync(CurrentAccountId);
            if (currentUser == null)
                return NotFound();
            var dto = new AccountProfileDTO();
            dto.Id = currentUser.Id;
            dto.Name = currentUser.Name;
            dto.Mail = currentUser.Mail;
            dto.Phone = currentUser.Phone;
            dto.ActivationTime = currentUser.ActivationTime;
            dto.ExpireTime = currentUser.ExpireTime;
            var organ = await _Context.Organizations.FirstOrDefaultAsync(x => x.Id == currentUser.OrganizationId);
            if (organ != null)
            {
                dto.Organization = organ.Name;
                dto.OrganizationId = organ.Id;
            }
            var role = await _Context.UserRoles.FirstOrDefaultAsync(x => x.Id == currentUser.InnerRoleId);
            if (role != null)
            {
                dto.Role = role.Name;
            }
            if (!string.IsNullOrWhiteSpace(currentUser.Icon))
            {
                var fs = await _Context.Files.FirstOrDefaultAsync(x => x.Id == currentUser.Icon);
                dto.Avatar = fs != null ? fs.Name : string.Empty;
            }
            return Ok(dto);
        }
        #endregion
    }
}