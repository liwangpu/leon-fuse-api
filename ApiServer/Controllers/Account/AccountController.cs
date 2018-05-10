using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace ApiServer.Controllers
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class AccountController : ListableController<Account>
    {
        private readonly ApiDbContext _Context;

        #region 构造函数
        public AccountController(ApiDbContext context)
        : base(new AccountStore(context))
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
            return await _GetPagingRequest(model);
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
            var mapping = new Func<Account, Task<Account>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Password = Md5.CalcString(model.Password);
                entity.Mail = model.Mail;
                entity.Frozened = false;
                var t1 = DataHelper.ParseDateTime(model.ActivationTime);
                var t2 = DataHelper.ParseDateTime(model.ExpireTime);
                entity.ActivationTime = t1 != DateTime.MinValue ? t1 : DateTime.Now;
                entity.ExpireTime = t2 != DateTime.MinValue ? t2 : DateTime.Now.AddYears(10);
                entity.Type = model.Type;
                entity.Location = model.Location;
                entity.Phone = model.Phone;
                entity.Mail = model.Mail;
                entity.OrganizationId = model.OrganizationId;
                entity.DepartmentId = model.DepartmentId;
                return Task.FromResult(entity);
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
            var mapping = new Func<Account, Task<Account>>((entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    entity.Password = Md5.CalcString(model.Password);
                entity.Mail = model.Mail;
                if (!string.IsNullOrWhiteSpace(model.ActivationTime))
                    entity.ActivationTime = DataHelper.ParseDateTime(model.ActivationTime);
                if (!string.IsNullOrWhiteSpace(model.ExpireTime))
                    entity.ExpireTime = DataHelper.ParseDateTime(model.ExpireTime);
                entity.Location = model.Location;
                entity.Phone = model.Phone;
                entity.Mail = model.Mail;
                entity.DepartmentId = model.DepartmentId;
                return Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
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
            Account acc = await _Context.Accounts.FindAsync(accid);
            if (acc == null)
                return null;
            AccountProfileModel p = new AccountProfileModel();
            p.Nickname = acc.Name;
            p.Avatar = acc.Icon;
            p.Brief = acc.Description;
            p.Location = acc.Location;
            p.OrganizationId = acc.OrganizationId;
            p.DepartmentId = acc.DepartmentId;
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
            Account acc = await _Context.Accounts.FindAsync(accid);
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