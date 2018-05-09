//using ApiModel.Entities;
//using ApiServer.Data;
//using ApiServer.Models;
//using ApiServer.Services;
//using ApiServer.Stores;
//using BambooCommon;
//using BambooCore;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace ApiServer.Controllers
//{
//    /// <summary>
//    /// 账号相关的接口
//    /// </summary>
//    [Authorize]
//    [Route("/[controller]")]
//    public class AccountController : Controller
//    {
//        AccountMan accountMan;
//        private readonly Repository<Account> repo;
//        private ApiDbContext _context;
//        private readonly AccountStore _AccountStore;
//        //private readonly AccountStore _AccountStore;
//        public AccountController(ApiDbContext context)
//        {
//            repo = new Repository<Account>(context);
//            accountMan = new AccountMan(context);
//            _context = context;
//            _AccountStore = new AccountStore(context);
//        }

//        #region Get 根据分页获取用户信息
//        /// <summary>
//        /// 根据分页获取用户信息
//        /// </summary>
//        /// <param name="search"></param>
//        /// <param name="page"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="orderBy"></param>
//        /// <param name="desc"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [ProducesResponseType(typeof(PagedData<AccountDTO>), 200)]
//        [ProducesResponseType(typeof(PagedData<AccountDTO>), 400)]
//        public async Task<PagedData<AccountDTO>> Get(string search, int page, int pageSize, string orderBy, bool desc)
//        {
//            var accid = AuthMan.GetAccountId(this);
//            return await _AccountStore.GetAccountByDepartmentAsync(accid, page, pageSize, orderBy, desc, search);
//        }
//        #endregion

//        #region Get 根据Id获取用户信息
//        /// <summary>
//        /// 根据Id获取用户信息
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(AccountDTO), 200)]
//        [ProducesResponseType(typeof(string), 400)]
//        public async Task<IActionResult> Get(string id)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);
//            var accid = AuthMan.GetAccountId(this);
//            var valid = await _AccountStore.CanRead(accid, id);
//            if (!string.IsNullOrWhiteSpace(valid))
//                return BadRequest(valid);
//            var dto = await _AccountStore.GetByIdAsync(accid, id);
//            return Ok(dto);
//        }
//        #endregion

//        #region Put 更新用户信息
//        /// <summary>
//        /// 更新用户信息
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPut]
//        [ProducesResponseType(typeof(AccountDTO), 200)]
//        [ProducesResponseType(typeof(string), 400)]
//        public async Task<IActionResult> Put([FromBody]AccountEditModel value)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);
//            if (string.IsNullOrWhiteSpace(value.Id))
//                return BadRequest(string.Format(ValidityMessage.V_RequiredRejectMsg, "id"));
//            var accid = AuthMan.GetAccountId(this);
//            var account = await _AccountStore.GetByIdAsync(value.Id);
//            account.Name = value.Name;
//            account.Description = value.Description;
//            if (!string.IsNullOrWhiteSpace(value.Password))
//                account.Password = Md5.CalcString(value.Password);
//            account.Mail = value.Mail;
//            account.ActivationTime = value.ActivationTime != null ? (DateTime)value.ActivationTime : DateTime.UtcNow;
//            account.ExpireTime = value.ExpireTime != null ? (DateTime)value.ExpireTime : DateTime.Now.AddYears(10);
//            account.Location = value.Location;
//            account.Phone = value.Phone;
//            account.Mail = value.Mail;
//            account.DepartmentId = value.DepartmentId;
//            account.Modifier = accid;
//            if (account == null)
//                return BadRequest(ValidityMessage.V_NotDataOrPermissionMsg);
//            var msg = await _AccountStore.CanUpdate(accid, account);
//            if (!string.IsNullOrWhiteSpace(msg))
//                return BadRequest(msg);
//            var dto = await _AccountStore.UpdateAsync(accid, account);
//            return Ok(dto);
//        }
//        #endregion

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
//            if (bOk)
//                return Ok();
//            return NotFound();//return Forbid();
//        }

//        #region Post 新建用户
//        /// <summary>
//        /// 新建用户
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ProducesResponseType(typeof(Organization), 200)]
//        [ProducesResponseType(typeof(string), 400)]
//        public async Task<IActionResult> Post([FromBody]RegisterAccountModel value)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var account = new Account();
//            account.Name = value.Name;
//            account.Description = value.Description;
//            account.Password = Md5.CalcString(value.Password);
//            account.Mail = value.Mail;
//            account.Frozened = false;
//            account.ActivationTime = value.ActivationTime != null ? (DateTime)value.ActivationTime : DateTime.UtcNow;
//            account.ExpireTime = value.ExpireTime != null ? (DateTime)value.ExpireTime : DateTime.Now.AddYears(10);
//            account.Type = value.Type;
//            account.Location = value.Location;
//            account.Phone = value.Phone;
//            account.Mail = value.Mail;
//            account.Creator = accid;
//            account.Modifier = accid;
//            account.OrganizationId = value.OrganizationId;
//            account.DepartmentId = value.DepartmentId;
//            var msg = await _AccountStore.CanCreate(accid, account);
//            if (!string.IsNullOrWhiteSpace(msg))
//                return BadRequest(msg);

//            var dto = await _AccountStore.CreateAsync(accid, account);
//            return Ok(dto);
//        }
//        #endregion


//        ///// <summary>
//        ///// 注册新账号
//        ///// </summary>
//        ///// <param name="value">新账号的基本信息</param>
//        ///// <returns></returns>
//        //[AllowAnonymous]
//        //[Route("Register")]
//        //[HttpPost]
//        //[Produces(typeof(Account))]
//        //public async Task<IActionResult> Register([FromBody]RegisterAccountModel value)
//        //{
//        //    if (ModelState.IsValid == false)
//        //        return BadRequest(ModelState);

//        //    var accid = AuthMan.GetAccountId(this);
//        //    var account = new Account();
//        //    account.Name = value.Name;
//        //    account.Description = value.Description;
//        //    account.Password = Md5.CalcString(value.Password);
//        //    account.Mail = value.Mail;
//        //    account.Frozened = false;
//        //    account.ActivationTime = value.ActivationTime != null ? value.ActivationTime : DateTime.UtcNow;
//        //    account.ExpireTime = value.ExpireTime != null ? value.ExpireTime : DateTime.Now.AddYears(10);
//        //    account.Type = value.Type;
//        //    account.Location = value.Location;
//        //    account.Phone = value.Phone;
//        //    account.Mail = value.Mail;
//        //    account.Creator = accid;
//        //    account.Modifier = accid;
//        //    account.OrganizationId = value.OrganizationId;
//        //    account.Organization = await _context.Organizations.FindAsync(value.OrganizationId);

//        //    var msg = await _AccountStore.CanCreate(accid, account);
//        //    if (msg.Count > 0)
//        //        return BadRequest(msg);

//        //    var dto = await _AccountStore.CreateAsync(accid, account);
//        //    return Ok(dto);
//        //}

//        /// <summary>
//        /// 重置密码
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [AllowAnonymous]
//        [Route("ResetPassword")]
//        [HttpPost]
//        public IActionResult ResetPassword([FromBody]ResetPasswordModel value)
//        {
//            //if (ModelState.IsValid == false)
//            //    return BadRequest(ModelState);
//            return Ok();
//        }

//        /// <summary>
//        /// 设置新密码
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [Route("NewPassword")]
//        [HttpPost]
//        public async Task<IActionResult> NewPassword([FromBody]NewPasswordModel value)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);
//            bool bOk = await accountMan.ChangePasswordAsync(AuthMan.GetAccountId(this), value);
//            if (bOk)
//                return Ok();
//            return BadRequest();
//        }

//        /// <summary>
//        /// 修改账号信息
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [Route("Profile")]
//        [HttpPost]
//        public async Task<IActionResult> EditProfile([FromBody]AccountProfileModel value)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);
//            bool bOk = await accountMan.UpdateProfile(AuthMan.GetAccountId(this), value);
//            if (bOk)
//                return Ok();
//            return BadRequest();
//        }

//        /// <summary>
//        /// 获取账号信息
//        /// </summary>
//        /// <returns></returns>
//        [Route("Profile")]
//        [HttpGet]
//        public async Task<AccountProfileModel> GetProfile()
//        {
//            return await accountMan.GetProfile(AuthMan.GetAccountId(this));
//        }

//        /// <summary>
//        /// 获取这个账号的网站后台导航菜单配置
//        /// </summary>
//        /// <returns></returns>
//        [Route("Navigation")]
//        [HttpGet]
//        public async Task<NavigationModel> GetNavigationData()
//        {
//            return await accountMan.GetNavigation(AuthMan.GetAccountId(this));
//        }
//    }
//}
