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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductGroupController : ListviewController<ProductGroup>
    {
        protected override AppDbContext _Context { get; }

        #region 构造函数
        public ProductGroupController(IRepository<ProductGroup> repository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
        }
        #endregion

        #region Get 根据分页获取产品组信息
        /// <summary>
        /// 根据分页获取产品组信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductGroupDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "", bool classify = true)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var advanceQuery = new Func<IQueryable<ProductGroup>, Task<IQueryable<ProductGroup>>>(async (query) =>
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

            var toDTO = new Func<ProductGroup, Task<ProductGroupDTO>>(async (entity) =>
            {
                var dto = new ProductGroupDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.PivotLocation = entity.PivotLocation;
                dto.PivotType = entity.PivotType;
                dto.Orientation = entity.Orientation;
                dto.Items = entity.Items;
                dto.CategoryId = entity.CategoryId;
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                if (!string.IsNullOrWhiteSpace(entity.CategoryId))
                {
                    var category = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == entity.CategoryId);
                    if (category != null)
                        dto.CategoryName = category.Name;
                }
                return await Task.FromResult(dto);
            });
            return await _PagingRequest(model, toDTO, advanceQuery);
        }
        #endregion

        #region Get 根据Id获取产品组信息
        /// <summary>
        /// 根据Id获取产品组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<ProductGroup, Task<ProductGroupDTO>>(async (entity) =>
            {
                var dto = new ProductGroupDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.PivotLocation = entity.PivotLocation;
                dto.PivotType = entity.PivotType;
                dto.Orientation = entity.Orientation;
                dto.Items = entity.Items;
                dto.CategoryId = entity.CategoryId;
                await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                {
                    dto.CreatorName = creatorName;
                    dto.ModifierName = modifierName;
                });
                if (!string.IsNullOrWhiteSpace(entity.CategoryId))
                {
                    var category = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == entity.CategoryId);
                    if (category != null)
                        dto.CategoryName = category.Name;
                }
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 新建产品组信息
        /// <summary>
        /// 新建产品组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductGroupCreateModel model)
        {
            var ProductGroupping = new Func<ProductGroup, Task<ProductGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.CategoryId = model.CategoryId;
                entity.PivotLocation = model.PivotLocation;
                entity.PivotType = model.PivotType;
                entity.Orientation = model.Orientation;
                entity.Items = model.Items;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(ProductGroupping);
        }
        #endregion

        #region Put 更新产品组信息
        /// <summary>
        /// 更新产品组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductGroupUpdateModel model)
        {
            var ProductGroupping = new Func<ProductGroup, Task<ProductGroup>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.CategoryId = model.CategoryId;
                entity.PivotLocation = model.PivotLocation;
                entity.PivotType = model.PivotType;
                entity.Orientation = model.Orientation;
                entity.Items = model.Items;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, ProductGroupping);
        }
        #endregion

        #region Delete 删除产品组信息
        /// <summary>
        /// 删除产品组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region BatchDelete 批量删除产品组项信息
        /// <summary>
        /// 批量删除产品组项信息
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