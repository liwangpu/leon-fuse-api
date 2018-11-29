using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Contexts;
using Apps.Basic.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers.Navigation
{
    /// <summary>
    /// 用户导航信息控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserNavController : ServiceBaseController<UserNav>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public UserNavController(IRepository<UserNav> repository, AppDbContext context)
        : base(repository)
        {
            _Context = context;
        }
        #endregion


        //[Route("GetUserNav")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserNav()
        //{
        //    //var accid = AuthMan.GetAccountId(this);
        //    //var account = await _Repository._DbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
        //    //var userNav = await _Repository._DbContext.UserNavs.Include(x => x.UserNavDetails).Where(x => x.Role == account.Type).FirstOrDefaultAsync();
        //    //if (userNav == null) return Ok(new List<UserNavDetail>());
        //    //var dto = await _Repository.GetByIdAsync(userNav.Id);
        //    //return Ok(dto.UserNavDetails);
        //    //_Context.
        //}
    }
}