﻿using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers.UIDesigner
{
    [Authorize]
    [Route("/[controller]")]
    public class NavigationController : ListableController<Navigation, NavigationDTO>
    {

        #region 构造函数
        public NavigationController(IRepository<Navigation, NavigationDTO> repository)
        : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取导航概要信息
        /// <summary>
        /// 根据分页查询信息获取导航概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<NavigationDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _GetPagingRequest(model);
        }
        #endregion

        #region Get 根据id获取导航信息
        /// <summary>
        /// 根据id获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NavigationDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Get 获取用户导航菜单
        /// <summary>
        /// 获取用户导航菜单
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Route("GetByRole")]
        [HttpGet]
        public async Task<IActionResult> GetByRole(string role)
        {
            var navs = _Repository._DbContext.Navigations.Select(x => x.ToDTO());
            return Ok(navs);
        }
        #endregion

        #region Post 新建导航信息
        /// <summary>
        /// 新建导航信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(NavigationDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]NavigationCreateModel model)
        {
            var mapping = new Func<Navigation, Task<Navigation>>(async (entity) =>
            {
                entity.Title = model.Title;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.Icon;
                entity.Url = model.Url;
                entity.Field = model.Field;
                entity.Permission = model.Permission;
                entity.PagedModel = model.PagedModel;
                entity.Resource = model.Resource;
                entity.NodeType = model.NodeType;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新导航信息
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(NavigationDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]NavigationEditModel model)
        {
            var mapping = new Func<Navigation, Task<Navigation>>(async (entity) =>
            {
                entity.Title = model.Title;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.Icon;
                entity.Url = model.Url;
                entity.Field = model.Field;
                entity.Permission = model.Permission;
                entity.PagedModel = model.PagedModel;
                entity.Resource = model.Resource;
                entity.NodeType = model.NodeType;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        //protected readonly ApiDbContext _Context;
        //public ITreeRepository<Navigation> _NavRepository { get; }

        //#region 构造函数
        //public NavigationController(ApiDbContext context, ITreeRepository<Navigation> navRepository)
        //{
        //    _Context = context;
        //    _NavRepository = navRepository;
        //}
        //#endregion

        //#region Get 获取用户导航菜单
        ///// <summary>
        ///// 获取用户导航菜单
        ///// </summary>
        ///// <param name="role"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> Get(string role)
        //{
        //    var navs = _Context.Navigations.Where(x => x.Role.ToLower() == role.ToLower()).Select(x => x.ToDTO());
        //    return Ok(navs);
        //} 
        //#endregion

        //#region Post 创建或更新用户角色导航菜单
        ///// <summary>
        ///// 创建或更新用户角色导航菜单
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateModel]
        //public async Task<IActionResult> Post([FromBody] NavigationCreateModel model)
        //{
        //    var accid = AuthMan.GetAccountId(this);
        //    var account = await _Context.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
        //    var rootNode = await _Context.PermissionTrees.FirstAsync(x => x.ObjId == account.OrganizationId);
        //    var metadata = new Navigation();
        //    metadata.Role = model.Role;
        //    metadata.Name = model.Name;
        //    metadata.Url = model.Url;
        //    metadata.Icon = model.Icon;
        //    metadata.NodeType = model.NodeType;
        //    metadata.Permission = model.Permission;
        //    metadata.Field = model.Field;
        //    metadata.PagedModel = model.PagedModel;
        //    metadata.Resource = model.Resource;
        //    metadata.ParentId = model.ParentId;
        //    metadata.OrganizationId = account.OrganizationId;
        //    metadata.RootOrganizationId = rootNode.RootOrganizationId;
        //    if (string.IsNullOrWhiteSpace(metadata.ParentId))
        //        await _NavRepository.AddNewNode(metadata);
        //    else
        //        await _NavRepository.AddChildNode(metadata);
        //    return Ok(metadata);
        //}
        //#endregion

    }
}