using ApiModel.Entities;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers.UIDesigner
{
    [Route("/[controller]")]
    public class NavigationController : ListableController<Navigation, NavigationDTO>
    {

        #region 构造函数
        public NavigationController(IRepository<Navigation, NavigationDTO> repository)
        : base(repository)
        {
        }
        #endregion


        #region Get 根据分页查询信息获取工作流程概要信息
        /// <summary>
        /// 根据分页查询信息获取工作流程概要信息
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