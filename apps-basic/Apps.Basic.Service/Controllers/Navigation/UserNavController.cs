using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 用户导航信息控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserNavController : ListviewController<UserNav>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public UserNavController(IRepository<UserNav> repository, AppDbContext context)
        : base(repository)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取用户导航栏信息
        /// <summary>
        /// 根据分页获取用户导航栏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<UserNavDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _PagingRequest(model);
        }
        #endregion

        #region Get 根据Id获取用户导航栏信息
        /// <summary>
        /// 根据Id获取用户导航栏信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserNavDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<UserNav, Task<UserNavDTO>>(async (entity) =>
            {
                var dto = new UserNavDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Role = entity.Role;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region GetUserNav 获取当前用户导航栏信息
        /// <summary>
        /// 获取当前用户导航栏信息
        /// </summary>
        /// <returns></returns>
        [Route("GetUserNav")]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserNavigationDTO>), 200)]
        public async Task<IActionResult> GetUserNav()
        {
            var dtos = new List<UserNavigationDTO>();
            var userNav = await _Context.UserNavs.Include(x => x.UserNavDetails).Where(x => x.Role == CurrentAccountInnerRoleId).FirstOrDefaultAsync();
            if (userNav == null || userNav.UserNavDetails == null || userNav.UserNavDetails.Count <= 0)
                return NoContent();
            var navigations = await _Context.Navigations.ToListAsync();
            foreach (var curNavItem in userNav.UserNavDetails)
            {
                var refNav = navigations.Where(x => x.Id == curNavItem.RefNavigationId).First();
                if (refNav == null) continue;
                var dto = new UserNavigationDTO();
                dto.Id = curNavItem.Id;
                dto.Title = refNav.Title;
                dto.Name = refNav.Name;
                dto.Url = refNav.Url;
                dto.Icon = refNav.Icon;
                dto.PagedModel = refNav.PagedModel;
                dto.NodeType = refNav.NodeType;
                dto.Resource = refNav.Resource;
                dto.NewTapOpen = refNav.NewTapOpen;
                dto.ParentId = curNavItem.ParentId;
                dto.Grade = curNavItem.Grade;
                if (!string.IsNullOrWhiteSpace(curNavItem.ExcludeQueryParams))
                {
                    var excludeArr = curNavItem.ExcludeQueryParams.Split(",");
                    var fullArr = refNav.QueryParams.Split(",");
                    var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
                    dto.QueryParams = string.Join(',', destArr);
                }
                else
                {
                    dto.QueryParams = refNav.QueryParams;
                }

                if (!string.IsNullOrWhiteSpace(curNavItem.ExcludeFiled))
                {
                    var excludeArr = curNavItem.ExcludeFiled.Split(",");
                    var fullArr = refNav.Field.Split(",");
                    var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
                    dto.Field = string.Join(',', destArr);
                }
                else
                {
                    dto.Field = refNav.Field;
                }

                if (!string.IsNullOrWhiteSpace(curNavItem.ExcludePermission))
                {
                    var excludeArr = curNavItem.ExcludePermission.Split(",");
                    var fullArr = refNav.Permission.Split(",");
                    var destArr = fullArr.Where(x => !excludeArr.Contains(x)).ToList();
                    dto.Permission = string.Join(',', destArr);
                }
                else
                {
                    dto.Permission = refNav.Permission;
                }
                dtos.Add(dto);
            }


            return Ok(dtos);
        }
        #endregion
    }
}