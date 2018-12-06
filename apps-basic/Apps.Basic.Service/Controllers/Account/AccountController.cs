﻿using Apps.Base.Common;
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
    public class AccountController : ListviewController<Account>
    {
        protected override AppDbContext _Context { get; }

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
        [ProducesResponseType(typeof(PagedData<AccountDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var toDTO = new Func<Account, Task<AccountDTO>>(async (entity) =>
            {
                var dto = new AccountDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Phone = entity.Phone;
                dto.Mail = entity.Mail;
                dto.ActivationTime = entity.ActivationTime;
                dto.ExpireTime = entity.ExpireTime;
                dto.Location = entity.Location;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
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
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<Account, Task<AccountDTO>>(async (entity) =>
            {
                var dto = new AccountDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Phone = entity.Phone;
                dto.Mail = entity.Mail;
                dto.Location = entity.Location;
                dto.ActivationTime = entity.ActivationTime;
                dto.ExpireTime = entity.ExpireTime;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.RoleId = entity.InnerRoleId;
                if (!string.IsNullOrWhiteSpace(entity.InnerRoleId))
                {
                    var role = await _Context.UserRoles.FirstOrDefaultAsync(x => x.Id == entity.InnerRoleId);
                    dto.RoleName = role != null ? role.Name : string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(entity.Icon))
                {
                    var fs = await _Context.Files.FirstOrDefaultAsync(x => x.Id == entity.Icon);
                    dto.Icon = fs != null ? fs.Url : string.Empty;
                }
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
                entity.Location = model.Location;
                entity.Phone = model.Phone;
                entity.ActivationTime = model.ActivationTime;
                entity.ExpireTime = model.ExpireTime;
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
        public async Task<IActionResult> Put([FromBody]AccountEditModel model)
        {
            var mapping = new Func<Account, Task<Account>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Mail = model.Mail;
                entity.Phone = model.Phone;
                entity.Location = model.Location;
                entity.ActivationTime = model.ActivationTime;
                entity.ExpireTime = model.ExpireTime;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    entity.Password = Md5.CalcString(model.Password);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
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
        [ProducesResponseType(typeof(AccountDTO), 200)]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = await _Context.Accounts.FindAsync(CurrentAccountId);
            if (currentUser == null) return NotFound();
            return await Get(currentUser.Id);
        }
        #endregion

        //#region ChangePassword 修改密码
        ///// <summary>
        ///// 修改密码
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("ChangePassword")]
        //[HttpPut]
        //[ValidateModel]
        //[ProducesResponseType(typeof(ValidationResultModel), 400)]
        //public async Task<IActionResult> ChangePassword([FromBody]NewPasswordModel model)
        //{
        //    var accid = AuthMan.GetAccountId(this);
        //    Account acc = await _Repository._DbContext.Accounts.FindAsync(accid);
        //    if (acc.Password != model.OldPassword)
        //        ModelState.AddModelError("Password", "原密码输入有误");
        //    if (!ModelState.IsValid)
        //        return new ValidationFailedResult(ModelState);
        //    acc.Password = model.NewPassword;
        //    _Repository._DbContext.Update(acc);
        //    await _Repository._DbContext.SaveChangesAsync();
        //    return Ok();
        //}
        //#endregion

        #region ResetPassword 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ResetPassword")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            var account = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == model.UserId);
            account.Password = model.Password;
            _Context.Accounts.Update(account);
            await _Context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}