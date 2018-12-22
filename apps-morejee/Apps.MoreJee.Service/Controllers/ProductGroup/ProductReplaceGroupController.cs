using Apps.Base.Common;
using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
using Apps.FileSystem.Export.Services;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Export.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductReplaceGroupController : ServiceBaseController<ProductReplaceGroup>
    {
        protected AppConfig _AppConfig { get; }

        #region 重构函数
        public ProductReplaceGroupController(IRepository<ProductReplaceGroup> repository, IOptions<AppConfig> settingsOptions)
         : base(repository)
        {
            _AppConfig = settingsOptions.Value;
        }
        #endregion

        #region Get 根据分页获取套餐信息
        /// <summary>
        /// 根据分页获取套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductReplaceGroupDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<ProductReplaceGroup, Task<ProductReplaceGroupDTO>>(async (entity) =>
            {
                var dto = new ProductReplaceGroupDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.OrganizationId = entity.OrganizationId;
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取套餐信息
        /// <summary>
        /// 根据Id获取套餐信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductReplaceGroupDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<ProductReplaceGroup, Task<ProductReplaceGroupDTO>>(async (entity) =>
            {
                var dto = new ProductReplaceGroupDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion
    }
}