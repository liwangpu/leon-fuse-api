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
            var toDTO = new Func<UserNav, Task<UserNavDTO>>(async (entity) =>
            {
                var dto = new UserNavDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Role = entity.Role;
                dto.Description = entity.Description;
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
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
                dto.Description = entity.Description;
                var userNavDetailDtos = new List<UserNavigationDTO>();
                if (entity.UserNavDetails != null)
                {
                    foreach (var item in entity.UserNavDetails)
                    {
                        var detailDto = new UserNavigationDTO();
                        detailDto.Id = item.Id;
                        detailDto.ExcludeFiled = item.ExcludeFiled;
                        detailDto.ExcludePermission = item.ExcludePermission;
                        detailDto.ExcludeQueryParams = item.ExcludeQueryParams;
                        detailDto.Grade = item.Grade;
                        detailDto.ParentId = item.ParentId;
                        var refNav = await _Context.Navigations.FirstOrDefaultAsync(x => x.Id == item.RefNavigationId);
                        if (refNav != null)
                        {
                            detailDto.Name = refNav.Name;
                            detailDto.Title = refNav.Title;
                            detailDto.Url = refNav.Url;
                            detailDto.NodeType = refNav.NodeType;
                            detailDto.Icon = refNav.Icon;
                            detailDto.Resource = refNav.Resource;
                            detailDto.Permission = refNav.PagedModel;
                            detailDto.Field = refNav.Field;
                            detailDto.QueryParams = refNav.QueryParams;
                            detailDto.NewTapOpen = refNav.NewTapOpen;
                            detailDto.Name = refNav.Name;
                            detailDto.Name = refNav.Name;
                        }
                        userNavDetailDtos.Add(detailDto);
                    }
                }
                dto.UserNavDetails = userNavDetailDtos;
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 创建导航栏项
        /// <summary>
        /// 创建导航栏项
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(UserNavDTO), 200)]
        public async Task<IActionResult> Post([FromBody]UserNavCreateModel model)
        {
            var mapping = new Func<UserNav, Task<UserNav>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Role = model.Role;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更改导航栏项信息
        /// <summary>
        /// 更改导航栏项信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(UserNavDTO), 200)]
        public async Task<IActionResult> Put([FromBody]UserNavEditModel model)
        {
            var mapping = new Func<UserNav, Task<UserNav>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Role = model.Role;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
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

        #region UpdateUserNavDetail 更新角色导航详细项信息
        /// <summary>
        /// 更新角色导航详细项信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateUserNavDetail")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(UserNavDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UpdateUserNavDetail([FromBody]UserNavDetailEditModel model)
        {
            var mapping = new Func<UserNav, Task<UserNav>>(async (entity) =>
            {
                var details = await _Context.UserNavDetails.Where(x => x.UserNav.Id == model.UserNavId).ToListAsync();
                var refDetail = !string.IsNullOrWhiteSpace(model.Id) ? details.Where(x => x.Id == model.Id).FirstOrDefault() : new UserNavDetail();
                if (refDetail == null)
                    refDetail = new UserNavDetail();


                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    if (string.IsNullOrWhiteSpace(model.ParentId))
                        refDetail.Grade = details.Where(x => string.IsNullOrWhiteSpace(x.ParentId)).Count();
                    else
                        refDetail.Grade = details.Where(x => x.ParentId == model.ParentId).Count();
                }
                refDetail.UserNav = entity;
                refDetail.ParentId = model.ParentId;
                refDetail.RefNavigationId = model.RefNavigationId;
                refDetail.ExcludeFiled = model.ExcludeFiled;
                refDetail.ExcludePermission = model.ExcludePermission;
                refDetail.ExcludeQueryParams = model.ExcludeQueryParams;

                if (!string.IsNullOrWhiteSpace(model.Id))
                {
                    _Context.UserNavDetails.Update(refDetail);
                }
                else
                {
                    refDetail.Id = GuidGen.NewGUID();
                    _Context.UserNavDetails.Add(refDetail);
                }
                await _Context.SaveChangesAsync();
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.UserNavId, mapping);
        }
        #endregion

        #region DeleteUserNavDetail 删除角色导航详细项信息
        /// <summary>
        /// 删除角色导航详细项信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteUserNavDetail")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(UserNavDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteUserNavDetail([FromBody]UserNavDetailDeleteModel model)
        {
            var mapping = new Func<UserNav, Task<UserNav>>(async (entity) =>
            {
                var details = await _Context.UserNavDetails.Where(x => x.UserNav.Id == model.UserNavId).ToListAsync();
                var deleteItem = details.Where(x => x.Id == model.Id).First();

                var deleteQueue = new Queue<UserNavDetail>();
                deleteQueue.Enqueue(deleteItem);

                do
                {
                    var it = deleteQueue.Dequeue();
                    var subs = details.Where(x => x.ParentId == it.Id).ToList();
                    subs.ForEach(t =>
                    {
                        deleteQueue.Enqueue(t);
                        details.Remove(t);
                        _Context.UserNavDetails.Remove(t);
                    });
                    details.Remove(it);
                    _Context.UserNavDetails.Remove(it);
                }
                while (deleteQueue.Count > 0);

                //重新排序同等级的导航栏信息
                var sameRankDetails = details.Where(x => x.ParentId == deleteItem.ParentId).OrderBy(x => x.Grade).ToList();
                for (int idx = sameRankDetails.Count - 1; idx >= 0; idx--)
                {
                    var item = sameRankDetails[idx];
                    item.Grade = idx;
                    _Context.UserNavDetails.Update(item);
                }

                await _Context.SaveChangesAsync();
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.UserNavId, mapping);
        }
        #endregion
    }
}