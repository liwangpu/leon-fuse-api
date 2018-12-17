using Apps.Base.Common;
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
using System.Linq;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Attributes;
using System.Collections.Generic;

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
                dto.Description = entity.Description;
                dto.DepartmentId = entity.DepartmentId;
                if (!string.IsNullOrWhiteSpace(entity.DepartmentId))
                {
                    var depName = await _Context.Departments.Where(x => x.Id == entity.DepartmentId).Select(x => x.Name).FirstOrDefaultAsync();
                    dto.DepartmentName = depName != null ? depName : string.Empty;
                }

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
            //HttpContext.Request.
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
                dto.Description = entity.Description;
                dto.RoleId = entity.InnerRoleId;
                dto.DepartmentId = entity.DepartmentId;
                if (!string.IsNullOrWhiteSpace(entity.InnerRoleId))
                {
                    var roleName = await _Context.UserRoles.Where(x => x.Id == entity.InnerRoleId).Select(x => x.Name).FirstOrDefaultAsync();
                    dto.RoleName = roleName != null ? roleName : string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(entity.DepartmentId))
                {
                    var depName = await _Context.Departments.Where(x => x.Id == entity.DepartmentId).Select(x => x.Name).FirstOrDefaultAsync();
                    dto.DepartmentName = depName != null ? depName : string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(entity.Icon))
                {
                    //var fsUrl = await _Context.Files.Where(x => x.Id == entity.Icon).Select(x => x.Url).FirstOrDefaultAsync();
                    //dto.Icon = fsUrl != null ? fsUrl : string.Empty;
                }
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region GetNameById 根据Id获取用户姓名
        /// <summary>
        /// 根据Id获取用户姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetNameById")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetNameById(string id)
        {
            var acc = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(acc != null ? acc.Name : string.Empty);
        }
        #endregion

        #region GetNameByIds 根据Ids获取用户姓名集合
        /// <summary>
        /// 根据Ids获取用户姓名集合
        /// </summary>
        /// <param name="ids">逗号分隔的id</param>
        /// <returns></returns>
        [Route("GetNameByIds")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IActionResult> GetNameByIds(string ids)
        {
            ids = string.IsNullOrWhiteSpace(ids) ? string.Empty : ids;
            var names = new List<string>();
            var idArr = ids.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in idArr)
            {
                var acc = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
                names.Add(acc != null ? acc.Name : string.Empty);
            }
            return Ok(names);
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
                entity.ActiveFlag = AppConst.Active;
                entity.Description = model.Description;
                entity.DepartmentId = string.IsNullOrWhiteSpace(model.DepartmentId) ? null : model.DepartmentId;
                entity.OrganizationId = CurrentAccountOrganizationId;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    entity.Password = Md5.CalcString(model.Password);

                var organTypeId = await _Context.Organizations.Where(x => x.Id == CurrentAccountOrganizationId).Select(x => x.OrganizationTypeId).FirstAsync();
                if (organTypeId == OrganTyeConst.Brand)
                    entity.InnerRoleId = UserRoleConst.BrandMember;
                else if (organTypeId == OrganTyeConst.Partner)
                    entity.InnerRoleId = UserRoleConst.PartnerMember;
                else if (organTypeId == OrganTyeConst.Supplier)
                    entity.InnerRoleId = UserRoleConst.SupplierMember;
                else { }

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
                entity.Description = model.Description;
                entity.DepartmentId = string.IsNullOrWhiteSpace(model.DepartmentId) ? null : model.DepartmentId;
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