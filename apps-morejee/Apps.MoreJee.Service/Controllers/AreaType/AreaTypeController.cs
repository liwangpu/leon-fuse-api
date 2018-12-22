using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
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
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AreaTypeController : ListviewController<AreaType>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public AreaTypeController(IRepository<AreaType> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取区域类型信息
        /// <summary>
        /// 根据分页获取区域类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<AreaTypeDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<AreaType, Task<AreaTypeDTO>>(async (entity) =>
            {
                var dto = new AreaTypeDTO();
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

        #region Get 根据Id获取区域类型信息
        /// <summary>
        /// 根据Id获取区域类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AreaTypeDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<AreaType, Task<AreaTypeDTO>>(async (entity) =>
            {
                var dto = new AreaTypeDTO();
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

        #region Post 新建区域类型信息
        /// <summary>
        /// 新建区域类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(AreaTypeDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]AreaTypeCreateModel model)
        {
            var AreaTypeping = new Func<AreaType, Task<AreaType>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(AreaTypeping);
        }
        #endregion

        #region Put 更新区域类型信息
        /// <summary>
        /// 更新区域类型信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(AreaTypeDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]AreaTypeUpdateModel model)
        {
            var AreaTypeping = new Func<AreaType, Task<AreaType>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, AreaTypeping);
        }
        #endregion

        #region Delete 删除区域类型信息
        /// <summary>
        /// 删除区域类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region BatchDelete 批量删除区域类型项信息
        /// <summary>
        /// 批量删除区域类型项信息
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