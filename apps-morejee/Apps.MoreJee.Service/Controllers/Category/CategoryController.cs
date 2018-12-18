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
    /// 资源分类控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ListviewController<AssetCategory>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public CategoryController(IRepository<AssetCategory> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
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
        [ProducesResponseType(typeof(PagedData<CategoryDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<AssetCategory, Task<CategoryDTO>>(async (entity) =>
            {
                var dto = new CategoryDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.Type = entity.Type;
                dto.DisplayIndex = entity.DisplayIndex;
                dto.ParentId = entity.ParentId;

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
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<AssetCategory, Task<CategoryDTO>>(async (entity) =>
            {
                var dto = new CategoryDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.Type = entity.Type;
                dto.DisplayIndex = entity.DisplayIndex;
                dto.ParentId = entity.ParentId;

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
    }
}