using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    /// <summary>
    /// 账号相关的接口
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class AccountController : Controller
    {
        AccountMan accountMan;
        private readonly Repository<Account> repo;
        private ApiDbContext _context;
        private readonly AccountStore _AccountStore;
        //private readonly AccountStore _AccountStore;
        public AccountController(ApiDbContext context)
        {
            repo = new Repository<Account>(context);
            accountMan = new AccountMan(context);
            _context = context;
            _AccountStore = new AccountStore(context);
        }

        [HttpGet]
        public async Task<PagedData<Account>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
               d => d.DepartmentId == search || d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(Account))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            repo.Context.Entry(res).Reference(d => d.Organization).Load();
            //repo.Context.Entry(res).Reference(d => d).Load();
            return Ok(res);//return Forbid();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(Account))]
        public async Task<IActionResult> Put([FromBody]AccountModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            //
            var refRecord = await _context.Accounts.FindAsync(value.Id);
            var entity = value.ToEntity();
            if (string.IsNullOrWhiteSpace(entity.Password))
                entity.Password = refRecord.Password;//密码如果为空,维持原来密码
            var res = await repo.UpdateAsync(AuthMan.GetAccountId(this), entity);
            if (res == null)
                return NotFound();
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
            if (bOk)
                return Ok();
            return NotFound();//return Forbid();
        }

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="value">新账号的基本信息</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        [Produces(typeof(Account))]
        public async Task<IActionResult> Register([FromBody]RegisterAccountModel value)
        {
            //if (ModelState.IsValid == false)
            //    return BadRequest(ModelState);

            //var accid = AuthMan.GetAccountId(this);
            //var department = new Account();
            //department.Name = value.Name;
            //department.Description = value.Description;
            //department.Creator = accid;
            //department.OrganizationId = value.OrganizationId;
            //department.Organization=await 

            //var msg = await _DepartmentStore.CanCreate(accid, department);
            //if (msg.Count > 0)
            //    return BadRequest(msg);

            //var dto = await _DepartmentStore.CreateAsync(accid, department);
            //return Ok(dto);

            //var refReor = await _context.Accounts.Where(x =>  x.Mail == value.Mail || x.Phone == value.Phone).FirstOrDefaultAsync();
            var refReor = await _context.Accounts.FirstOrDefaultAsync(x => !string.IsNullOrWhiteSpace(value.Mail) && x.Mail == value.Mail || !string.IsNullOrWhiteSpace(value.Phone) && x.Phone == value.Phone);
            if (refReor != null)
                return BadRequest("账户信息已经存在,修改您的邮箱或者电话信息");
            AccountModel account = await accountMan.Register(value);
            repo.Context.Set<PermissionItem>().Add(Permission.NewItem(AuthMan.GetAccountId(this), account.Id, value.GetType().Name, PermissionType.All));
            if (value.Type == AppConst.AccountType_OrganAdmin)
                repo.Context.Set<PermissionItem>().Add(Permission.NewItem(account.Id, value.DepartmentId, "Department", PermissionType.All));

            await repo.SaveChangesAsync();
            if (account == null)
                return BadRequest();
            return Ok(account);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody]ResetPasswordModel value)
        {
            //if (ModelState.IsValid == false)
            //    return BadRequest(ModelState);
            return Ok();
        }

        /// <summary>
        /// 设置新密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("NewPassword")]
        [HttpPost]
        public async Task<IActionResult> NewPassword([FromBody]NewPasswordModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            bool bOk = await accountMan.ChangePasswordAsync(AuthMan.GetAccountId(this), value);
            if (bOk)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("Profile")]
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromBody]AccountProfileModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            bool bOk = await accountMan.UpdateProfile(AuthMan.GetAccountId(this), value);
            if (bOk)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <returns></returns>
        [Route("Profile")]
        [HttpGet]
        public async Task<AccountProfileModel> GetProfile()
        {
            return await accountMan.GetProfile(AuthMan.GetAccountId(this));
        }

        /// <summary>
        /// 获取这个账号的网站后台导航菜单配置
        /// </summary>
        /// <returns></returns>
        [Route("Navigation")]
        [HttpGet]
        public async Task<NavigationModel> GetNavigationData()
        {
            return await accountMan.GetNavigation(AuthMan.GetAccountId(this));
        }
    }
}
