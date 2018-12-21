using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Export.Services;
using Apps.FileSystem.Export.Services;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Export.DTOs;
using Apps.MoreJee.Export.Models;
using Apps.MoreJee.Export.Services;
using Apps.MoreJee.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    /// <summary>
    /// 产品控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ListviewController<Product>
    {
        protected override AppDbContext _Context { get; }
        protected IRepository<ProductSpec> _ProductSpecRep { get; }

        #region 构造函数
        public ProductController(IRepository<Product> repository, IRepository<ProductSpec> specRpository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository, settingsOptions)
        {
            _Context = context;
            _ProductSpecRep = specRpository;
        }
        #endregion

        #region Get 根据分页获取产品信息
        /// <summary>
        /// 根据分页获取产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Product, Task<ProductDTO>>(async (entity) =>
            {
                var dto = new ProductDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Unit = entity.Unit;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
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
            return await _PagingRequest(model, toDTO);
        }
        #endregion

        #region Get 根据Id获取产品信息
        /// <summary>
        /// 根据Id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            var fileMicroService = new FileMicroService(_AppConfig.APIGatewayServer, Token);
            var specMicroService = new ProductSpecMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<Product, Task<ProductDTO>>(async (entity) =>
            {
                var dto = new ProductDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Unit = entity.Unit;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.CategoryId = entity.CategoryId;
                dto.ActiveFlag = entity.ActiveFlag;
                dto.IconAssetId = entity.Icon;
                dto.Specifications = new List<ProductSpecDTO>();
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
                await fileMicroService.GetUrlById(entity.Icon, (url) =>
                {
                    dto.Icon = url;
                });
                if (entity.Specifications != null && entity.Specifications.Count > 0)
                {
                    foreach (var spec in entity.Specifications)
                    {
                        var specDto =await specMicroService.GetById(spec.Id);
                        dto.Specifications.Add(specDto);
                    }
                }



                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region Post 新建产品信息
        /// <summary>
        /// 新建产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductCreateModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Unit = model.Unit;
                entity.CategoryId = model.CategoryId;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });

            var afterCreated = new Func<Product, Task>(async (entity) =>
            {
                //创建默认的规格
                var defaultSpec = new ProductSpec();
                defaultSpec.Name = "默认";
                defaultSpec.ProductId = entity.Id;
                defaultSpec.OrganizationId = entity.OrganizationId;
                await _ProductSpecRep.CreateAsync(defaultSpec, CurrentAccountId);
                //将该规格设为产品默认规格
                entity.DefaultSpecId = defaultSpec.Id;
                _Context.Products.Update(entity);
                await _Context.SaveChangesAsync();

            });
            return await _PostRequest(mapping, afterCreated);
        }
        #endregion

        #region Put 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductUpdateModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                entity.Unit = model.Unit;
                entity.CategoryId = model.CategoryId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Delete 删除产品信息
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            return await _DeleteRequest(id);
        }
        #endregion

        #region BatchDelete 批量删除产品项信息
        /// <summary>
        /// 批量删除产品项信息
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