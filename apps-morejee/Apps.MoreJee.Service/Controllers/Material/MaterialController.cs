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
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        #region Get 根据分页获取材质信息
        /// <summary>
        /// 根据分页获取材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MaterialDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "", bool classify = true)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var advanceQuery = new Func<IQueryable<Material>, Task<IQueryable<Material>>>(async (query) => 
            {
                if (classify)
                {
                    if (!string.IsNullOrWhiteSpace(categoryId))
                    {
                        var curCategoryTree = await _Context.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == categoryId);
                        //如果是根节点,把所有取出,不做分类过滤
                        if (curCategoryTree != null && curCategoryTree.LValue > 1)
                        {
                            var categoryQ = from it in _Context.AssetCategoryTrees
                                            where it.NodeType == curCategoryTree.NodeType && it.OrganizationId == curCategoryTree.OrganizationId
                                            && it.LValue >= curCategoryTree.LValue && it.RValue <= curCategoryTree.RValue
                                            select it;
                            query = from it in query
                                    join cat in categoryQ on it.CategoryId equals cat.ObjId
                                    select it;
                        }
                    }
                }
                else
                {
                    query = query.Where(x => string.IsNullOrWhiteSpace(x.CategoryId));
                }
                return query;
            });

             var toDTO = new Func<Material, Task<MaterialDTO>>(async (entity) =>
            {
                var dto = new MaterialDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.FileAssetId = entity.FileAssetId;
                dto.Parameters = entity.Parameters;
                dto.Dependencies = entity.Dependencies;
                dto.PackageName = entity.PackageName;
                dto.UnCookedAssetId = entity.UnCookedAssetId;
                dto.OrganizationId = entity.OrganizationId;
                dto.CategoryId = entity.CategoryId;
                dto.ActiveFlag = entity.ActiveFlag;
                if (!string.IsNullOrWhiteSpace(entity.CategoryId))
                {
                    var category = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == entity.CategoryId);
                    if (category != null)
                        dto.CategoryName = category.Name;
                }
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
            return await _PagingRequest(model, toDTO, advanceQuery);
        }
        #endregion

        #region Get 根据Id获取材质信息
        /// <summary>
        /// 根据Id获取材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Material, Task<MaterialDTO>>(async (entity) =>
            {
                var dto = new MaterialDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.FileAssetId = entity.FileAssetId;
                dto.Parameters = entity.Parameters;
                dto.Dependencies = entity.Dependencies;
                dto.Template = entity.Template;
                dto.PackageName = entity.PackageName;
                dto.UnCookedAssetId = entity.UnCookedAssetId;
                dto.OrganizationId = entity.OrganizationId;
                dto.CategoryId = entity.CategoryId;
                dto.ActiveFlag = entity.ActiveFlag;
                dto.IconAssetId = entity.Icon;
                if (!string.IsNullOrWhiteSpace(entity.CategoryId))
                {
                    var category = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == entity.CategoryId);
                    if (category != null)
                        dto.CategoryName = category.Name;
                }
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

        #region Post 新建材质信息
        /// <summary>
        /// 新建材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]MaterialCreateModel model)
        {
            var mapping = new Func<Material, Task<Material>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Icon = model.IconAssetId;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.PackageName = model.PackageName;
                entity.UnCookedAssetId = model.UnCookedAssetId;
                entity.CategoryId = model.CategoryId;
                entity.Dependencies = model.Dependencies;
                entity.Parameters = model.Parameters;
                entity.Template = model.Template;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新材质信息
        /// <summary>
        /// 更新材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]MaterialUpdateModel model)
        {
            var mapping = new Func<Material, Task<Material>>(async (entity) =>
            {
                entity.Name = model.Name;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                entity.Description = model.Description;
                entity.FileAssetId = model.FileAssetId;
                entity.PackageName = model.PackageName;
                entity.UnCookedAssetId = model.UnCookedAssetId;
                entity.CategoryId = model.CategoryId;
                entity.Dependencies = model.Dependencies;
                entity.Parameters = model.Parameters;
                entity.Template = model.Template;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Delete 删除材质信息
        /// <summary>
        /// 删除材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region BatchDelete 批量删除材质项信息
        /// <summary>
        /// 批量删除材质项信息
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