using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Apps.Basic.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Controllers
{
    /// <summary>
    /// 导航栏项控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NavigationController : ListviewController<Navigation>
    {
        #region 构造函数
        public NavigationController(IRepository<Navigation> repository)
            : base(repository)
        {
        }
        #endregion

        #region Get 根据分页获取导航栏项信息
        /// <summary>
        /// 根据分页获取导航栏项信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<NavigationDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            return await _PagingRequest(model);
        }
        #endregion

        #region Get 根据Id获取导航栏项信息
        /// <summary>
        /// 根据Id获取导航栏项信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NavigationDTO), 200)]
        public override async Task<IActionResult> Get(string id)
        {
            var toDTO = new Func<Navigation, Task<NavigationDTO>>(async (entity) =>
            {
                var dto = new NavigationDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Title = entity.Title;
                dto.NodeType = entity.NodeType;
                dto.Icon = entity.Icon;
                dto.Url = entity.Url;
                dto.Resource = entity.Resource;
                dto.Permission = entity.Permission;
                dto.PagedModel = entity.PagedModel;
                dto.Field = entity.Field;
                dto.QueryParams = entity.QueryParams;
                dto.NewTapOpen = entity.NewTapOpen;
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
        [ProducesResponseType(typeof(AccountDTO), 200)]
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
                entity.QueryParams = model.QueryParams;
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
        [ProducesResponseType(typeof(AccountDTO), 200)]
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
                entity.QueryParams = model.QueryParams;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Delete 删除导航栏项信息
        /// <summary>
        /// 删除导航栏项信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region BatchDelete 批量删除导航栏项信息
        /// <summary>
        /// 批量删除导航栏项信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("BatchDelete")]
        [HttpDelete]
        public virtual async Task<IActionResult> BatchDelete(string ids)
        {
            return await _BatchDeleteRequest(ids);
        }
        #endregion

    }
}