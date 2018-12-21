using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
using Apps.FileSystem.Export.Services;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Export.DTOs;
using Apps.MoreJee.Export.Models;
using Apps.MoreJee.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    /// <summary>
    /// 场景控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MapController : ListviewController<Map>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public MapController(IRepository<Map> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取场景信息
        /// <summary>
        /// 根据分页获取场景信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MapDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Map, Task<MapDTO>>(async (entity) =>
            {
                var dto = new MapDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.FileAssetId = entity.FileAssetId;
                dto.Dependencies = entity.Dependencies;
                dto.Properties = entity.Properties;
                dto.PackageName = entity.PackageName;
                dto.UnCookedAssetId = entity.UnCookedAssetId;
                dto.OrganizationId = entity.OrganizationId;

                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                await fileMicroServer.GetUrlById(entity.Icon, (url) =>
                {
                    dto.Icon = url;
                });
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取场景信息
        /// <summary>
        /// 根据Id获取场景信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MapDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Map, Task<MapDTO>>(async (entity) =>
            {
                var dto = new MapDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.FileAssetId = entity.FileAssetId;
                dto.Dependencies = entity.Dependencies;
                dto.Properties = entity.Properties;
                dto.PackageName = entity.PackageName;
                dto.UnCookedAssetId = entity.UnCookedAssetId;
                dto.OrganizationId = entity.OrganizationId;
                dto.IconAssetId = entity.Icon;
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                await fileMicroServer.GetUrlById(entity.Icon, (url) =>
                 {
                     dto.Icon = url;
                 });
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 新建场景信息
        /// <summary>
        /// 新建场景信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MapCreateModel model)
        {
            var mapping = new Func<Map, Task<Map>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.PackageName = model.PackageName;
                entity.UnCookedAssetId = model.UnCookedAssetId;
                entity.FileAssetId = model.FileAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新场景信息
        /// <summary>
        /// 更新场景信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MapEditModel model)
        {
            var mapping = new Func<Map, Task<Map>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.PackageName = model.PackageName;
                entity.UnCookedAssetId = model.UnCookedAssetId;
                entity.FileAssetId = model.FileAssetId;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                entity.Dependencies = model.Dependencies;
                entity.Properties = model.Properties;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Delete 删除场景信息
        /// <summary>
        /// 删除场景信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        } 
        #endregion

        #region BatchDelete 批量删除场景项信息
        /// <summary>
        /// 批量删除场景项信息
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