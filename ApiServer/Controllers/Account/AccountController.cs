using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class AccountController : ListableController<Account, AccountDTO>
    {

        #region 构造函数
        public AccountController(IRepository<Account, AccountDTO> repository)
        : base(repository)
        {
        }
        #endregion

        #region _GetAccountType 获取用户类型信息
        /// <summary>
        /// 获取用户类型信息
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected async Task<string> _GetAccountType(bool isAdmin, string accountId = "")
        {
            if (string.IsNullOrWhiteSpace(accountId))
                accountId = AuthMan.GetAccountId(this);
            var account = await _Repository._DbContext.Accounts.Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == accountId);
            if (account != null && account.Organization != null)
            {
                if (isAdmin)
                    return $"{account.Organization.Type}admin";
                else
                    return $"{account.Organization.Type}member";
            }
            return string.Empty;
        }
        #endregion

        #region Get 根据分页获取用户信息
        /// <summary>
        /// 根据分页获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="departmentId"></param>
        /// <param name="ignoreOwner"></param>
        /// <param name="currentOrganAccount"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<AccountDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string departmentId, bool ignoreOwner = false, bool currentOrganAccount = true)
        {
            var organId = await _GetCurrentUserOrganId();

            var advanceQuery = new Func<IQueryable<Account>, Task<IQueryable<Account>>>(async (query) =>
            {
                if (!string.IsNullOrWhiteSpace(departmentId))
                {
                    query = query.Where(x => x.DepartmentId == departmentId);
                }
                if (ignoreOwner)
                {
                    var organ = await _Repository._DbContext.Organizations.FindAsync(organId);
                    if (organ != null)
                    {
                        query = query.Where(x => x.Id != organ.OwnerId);
                    }
                }
                if (currentOrganAccount)
                {
                    query = query.Where(x => x.OrganizationId == organId);
                }
                query = query.Where(x => x.ActiveFlag == AppConst.I_DataState_Active);
                return query;
            });
            return await _GetPagingRequest(model, null, advanceQuery);
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
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建用户
        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]RegisterAccountModel model)
        {
            var mapping = new Func<Account, Task<Account>>(async (entity) =>
            {
                if (string.IsNullOrWhiteSpace(model.OrganizationId))
                    model.OrganizationId = await _GetCurrentUserOrganId();

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Password = Md5.CalcString(model.Password);
                entity.Mail = model.Mail;
                entity.Frozened = false;
                var t1 = DataHelper.ParseDateTime(model.ActivationTime);
                var t2 = DataHelper.ParseDateTime(model.ExpireTime);
                entity.ActivationTime = t1 != DateTime.MinValue ? t1 : DateTime.Now;
                entity.ExpireTime = t2 != DateTime.MinValue ? t2 : DateTime.Now.AddYears(10);
                entity.Type = await _GetAccountType(model.IsAdmin);
                entity.Location = model.Location;
                entity.Phone = model.Phone;
                entity.Mail = model.Mail;
                entity.OrganizationId = model.OrganizationId;
                entity.DepartmentId = model.DepartmentId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新用户信息
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(AccountDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]AccountEditModel model)
        {
            var mapping = new Func<Account, Task<Account>>(async (entity) =>
            {
                if (!string.IsNullOrWhiteSpace(model.Name))
                    entity.Name = model.Name;
                if (!string.IsNullOrWhiteSpace(model.Description))
                    entity.Description = model.Description;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    entity.Password = Md5.CalcString(model.Password);
                if (!string.IsNullOrWhiteSpace(model.ActivationTime))
                    entity.ActivationTime = DataHelper.ParseDateTime(model.ActivationTime);
                if (!string.IsNullOrWhiteSpace(model.ExpireTime))
                    entity.ExpireTime = DataHelper.ParseDateTime(model.ExpireTime);


                entity.DepartmentId = model.DepartmentId;
                entity.Mail = model.Mail;
                entity.Location = model.Location;
                entity.Phone = model.Phone;

                entity.Type = await _GetAccountType(model.IsAdmin, model.Id);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region ChangePassword 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> ChangePassword([FromBody]NewPasswordModel model)
        {
            var accid = AuthMan.GetAccountId(this);
            Account acc = await _Repository._DbContext.Accounts.FindAsync(accid);
            if (acc.Password != model.OldPassword)
                ModelState.AddModelError("Password", "原密码输入有误");
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            acc.Password = model.NewPassword;
            _Repository._DbContext.Update(acc);
            await _Repository._DbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

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
            var accid = AuthMan.GetAccountId(this);
            Account acc = await _Repository._DbContext.Accounts.FindAsync(model.UserId);
            acc.Password = model.Password;
            _Repository._DbContext.Update(acc);
            await _Repository._DbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region GetProfile 获取账号信息
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <returns></returns>
        [Route("Profile")]
        [HttpGet]
        [ProducesResponseType(typeof(AccountProfileModel), 200)]
        public async Task<AccountProfileModel> GetProfile()
        {
            var accid = AuthMan.GetAccountId(this);
            Account acc = await _Repository._DbContext.Accounts.FindAsync(accid);
            if (acc == null)
                return null;
            AccountProfileModel p = new AccountProfileModel();
            p.Id = acc.Id;
            p.Name = acc.Name;
            if (!string.IsNullOrWhiteSpace(acc.Icon))
            {
                var fs = await _Repository._DbContext.Files.FirstOrDefaultAsync(x => x.Id == acc.Icon);
                p.Avatar = fs.Url;
            }
            p.Brief = acc.Description;
            p.Location = acc.Location;
            p.Mail = acc.Mail;
            p.Phone = acc.Phone;
            p.ActivationTime = acc.ActivationTime;
            p.ExpireTime = acc.ExpireTime;
            p.OrganizationId = acc.OrganizationId;
            var organ = await _Repository._DbContext.Organizations.FirstOrDefaultAsync(x => x.Id == acc.OrganizationId);
            p.Organization = organ != null ? organ.Name : "";
            p.DepartmentId = acc.DepartmentId;
            if (!string.IsNullOrWhiteSpace(p.DepartmentId))
            {
                var dep = await _Repository._DbContext.Departments.FirstOrDefaultAsync(x => x.Id == p.DepartmentId);
                p.Department = dep != null ? dep.Name : "";
            }
            p.Role = acc.Type;
            return p;
        }
        #endregion

        #region GetNavigationData 获取这个账号的网站后台导航菜单配置
        /// <summary>
        /// 获取这个账号的网站后台导航菜单配置
        /// </summary>
        /// <returns></returns>
        [Route("Navigation")]
        [HttpGet]
        [ProducesResponseType(typeof(NavigationModel), 200)]
        public async Task<NavigationModel> GetNavigationData()
        {
            var accid = AuthMan.GetAccountId(this);
            Account acc = await _Repository._DbContext.Accounts.FindAsync(accid);
            if (acc != null)
            {
                NavigationModel mm;
                if (SiteConfig.Instance.GetItem("navi_" + acc.Type, out mm))
                    return mm;
            }
            return null;
        }
        #endregion
    }
}