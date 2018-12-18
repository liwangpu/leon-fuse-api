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
    public class MaterialController : ListviewController<Material>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public MaterialController(IRepository<Material> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取场景信息
        ///// <summary>
        ///// 根据分页获取场景信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(typeof(PagedData<MapDTO>), 200)]
        //public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        //{
        //    var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);

        //    var toDTO = new Func<Map, Task<MapDTO>>(async (entity) =>
        //    {
        //        var dto = new MapDTO();
        //        dto.Id = entity.Id;
        //        dto.Name = entity.Name;
        //        dto.Description = entity.Description;
        //        dto.Creator = entity.Creator;
        //        dto.Modifier = entity.Modifier;
        //        dto.CreatedTime = entity.CreatedTime;
        //        dto.ModifiedTime = entity.ModifiedTime;
        //        dto.FileAssetId = entity.FileAssetId;
        //        dto.Dependencies = entity.Dependencies;
        //        dto.Properties = entity.Properties;
        //        dto.PackageName = entity.PackageName;
        //        dto.UnCookedAssetId = entity.UnCookedAssetId;
        //        dto.OrganizationId = entity.OrganizationId;

        //        await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
        //        {
        //            dto.CreatorName = creatorName;
        //            dto.ModifierName = modifierName;
        //        });
        //        return await Task.FromResult(dto);
        //    });
        //    return await _PagingRequest(model, toDTO);
        //}
        #endregion

        public override Task<IActionResult> Get(string id)
        {
            throw new NotImplementedException();
        }

    }
}