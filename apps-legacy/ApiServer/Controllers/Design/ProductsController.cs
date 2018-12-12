﻿using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Controllers.Common;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Repositories;
using ApiServer.Services;
using BambooCore;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductsController : ResourceController<Product, ProductDTO>
    {
        public override int ResType => ResourceTypeConst.Product;

        #region 构造函数
        public ProductsController(IRepository<Product, ProductDTO> repository)
            : base(repository)
        {
        }
        #endregion

        #region Get 根据分页查询信息获取产品概要信息
        /// <summary>
        /// 根据分页查询信息获取产品概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoryName"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "", string categoryName = "", bool classify = true)
        {
            var advanceQuery = new Func<IQueryable<Product>, Task<IQueryable<Product>>>(async (query) =>
            {
                if (classify)
                {
                    #region 根据分类Id查询
                    if (!string.IsNullOrWhiteSpace(categoryId))
                    {
                        var curCategoryTree = await _Repository._DbContext.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == categoryId);
                        //如果是根节点,把所有取出,不做分类过滤
                        if (curCategoryTree != null && curCategoryTree.LValue > 1)
                        {
                            var categoryQ = from it in _Repository._DbContext.AssetCategoryTrees
                                            where it.NodeType == curCategoryTree.NodeType && it.OrganizationId == curCategoryTree.OrganizationId
                                            && it.LValue >= curCategoryTree.LValue && it.RValue <= curCategoryTree.RValue
                                            select it;
                            query = from it in query
                                    join cat in categoryQ on it.CategoryId equals cat.ObjId
                                    select it;
                        }
                    }
                    //else
                    //{
                    //    query = query.Where(x => !string.IsNullOrWhiteSpace(x.CategoryId));
                    //}
                    #endregion

                    #region 根据分类名称查询
                    if (!string.IsNullOrWhiteSpace(categoryName))
                    {
                        var accid = AuthMan.GetAccountId(this);
                        var account = await _Repository._DbContext.Accounts.FindAsync(accid);
                        var categoryIds = _Repository._DbContext.AssetCategories.Where(x => x.Type == AppConst.S_Category_Product && x.OrganizationId == account.OrganizationId && x.Name.Contains(categoryName)).Select(x => x.Id);
                        query = query.Where(x => categoryIds.Contains(x.CategoryId));
                    }
                    #endregion
                }
                else
                {
                    query = query.Where(x => string.IsNullOrWhiteSpace(x.CategoryId));
                }
                query = query.Where(x => x.ActiveFlag == AppConst.I_DataState_Active);
                return await Task.FromResult(query);
            });

            return await _GetPagingRequest(model, null, advanceQuery);
        }
        #endregion

        #region Get 根据id获取产品信息
        /// <summary>
        /// 根据id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
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
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;


                #region 自动创建默认的规格
                {
                    var accid = AuthMan.GetAccountId(this);
                    var account = await _Repository._DbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
                    var defaultSpec = new ProductSpec();
                    defaultSpec.Id = GuidGen.NewGUID();
                    defaultSpec.Name = entity.Name;
                    defaultSpec.Product = entity;
                    defaultSpec.Price = model.Price;
                    defaultSpec.PurchasePrice = model.PurchasePrice;
                    defaultSpec.PartnerPrice = model.PartnerPrice;
                    defaultSpec.OrganizationId = account.OrganizationId;
                    defaultSpec.Creator = accid;
                    defaultSpec.Modifier = accid;
                    defaultSpec.CreatedTime = DateTime.UtcNow;
                    defaultSpec.ModifiedTime = DateTime.UtcNow;
                    entity.Specifications = new List<ProductSpec>() { defaultSpec };
                }
                #endregion
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
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
        public async Task<IActionResult> Put([FromBody]ProductEditModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Unit = model.Unit;
                entity.CategoryId = model.CategoryId;

                var defaultSpec = await _Repository._DbContext.ProductSpec.Where(x => x.ProductId == entity.Id && x.ActiveFlag == AppConst.I_DataState_Active).OrderByDescending(x => x.CreatedTime).FirstOrDefaultAsync();

                var accid = AuthMan.GetAccountId(this);
                if (defaultSpec != null)
                {
                    defaultSpec.Name = entity.Name;
                    defaultSpec.Price = model.Price;
                    defaultSpec.PurchasePrice = model.PurchasePrice;
                    defaultSpec.PartnerPrice = model.PartnerPrice;
                    defaultSpec.Modifier = accid;
                    defaultSpec.ModifiedTime = DateTime.UtcNow;
                }
                else
                {
                    var account = await _Repository._DbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
                    defaultSpec = new ProductSpec();
                    defaultSpec.Id = GuidGen.NewGUID();
                    defaultSpec.Name = entity.Name;
                    defaultSpec.Product = entity;
                    defaultSpec.Price = model.Price;
                    defaultSpec.PurchasePrice = model.PurchasePrice;
                    defaultSpec.PartnerPrice = model.PartnerPrice;
                    defaultSpec.OrganizationId = account.OrganizationId;
                    defaultSpec.Creator = accid;
                    defaultSpec.Modifier = accid;
                    defaultSpec.CreatedTime = DateTime.UtcNow;
                    defaultSpec.ModifiedTime = DateTime.UtcNow;
                    entity.Specifications = new List<ProductSpec>() { defaultSpec };
                }


                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region BulkChangeCategory 批量修改产品分类信息
        /// <summary>
        /// 批量修改产品分类信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("BulkChangeCategory")]
        [ValidateModel]
        [HttpPut]
        public async Task<IActionResult> BulkChangeCategory([FromBody]BulkChangeCategoryModel model)
        {
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            var existCategory = await _Repository._DbContext.AssetCategories.CountAsync(x => x.Id == model.CategoryId) > 0;
            if (!existCategory)
            {
                ModelState.AddModelError("CategoryId", "对应记录不存在");
                return new ValidationFailedResult(ModelState);
            }
            var idArr = model.Ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            using (var transaction = _Repository._DbContext.Database.BeginTransaction())
            {
                try
                {
                    for (int idx = idArr.Length - 1; idx >= 0; idx--)
                    {
                        var id = idArr[idx];
                        var refProduct = await _Repository._DbContext.Products.FindAsync(id);
                        if (refProduct != null)
                        {
                            refProduct.CategoryId = model.CategoryId;
                            _Repository._DbContext.Products.Update(refProduct);
                        }
                        else
                        {
                            ModelState.AddModelError("ProductId", "对应记录不存在");
                            return new ValidationFailedResult(ModelState);
                        }
                    }
                    _Repository._DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return Ok();
        }
        #endregion

        #region ImportProductAndCategory 根据CSV批量分类产品
        /// <summary>
        /// 根据CSV批量分类产品
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("ImportProductAndCategory")]
        [HttpPut]
        public async Task<IActionResult> ImportProductAndCategory(IFormFile file)
        {
            var accid = AuthMan.GetAccountId(this);
            var currentAcc = await _Repository._DbContext.Accounts.FindAsync(accid);
            var psProductQuery = await _Repository._GetPermissionData(accid, DataOperateEnum.Update);
            var psCategoryQuery = _Repository._DbContext.AssetCategories.Where(x => x.Type == AppConst.S_Category_Product && x.ActiveFlag == AppConst.I_DataState_Active && x.OrganizationId == currentAcc.OrganizationId);
            var importOp = new Func<ProductAndCategoryImportCSV, Task<string>>(async (data) =>
            {
                var mapProductCount = await psProductQuery.Where(x => x.Name.Trim() == data.ProductName.Trim()).CountAsync();
                if (mapProductCount == 0)
                    return "没有找到该产品或您没有权限修改该条数据";
                if (mapProductCount > 1)
                    return "产品名称有重复,请手动分配该产品";
                var mapCategoryCount = await psCategoryQuery.Where(x => x.Name == data.CategoryName).CountAsync();
                if (mapCategoryCount == 0)
                    return "没有找到该分类,请确认分类名称是否有误";
                if (mapCategoryCount > 1)
                    return "分类名称有重复,请手动分配该产品";

                var refProduct = await psProductQuery.Where(x => x.Name.Trim() == data.ProductName.Trim()).FirstAsync();
                var refCategory = await psCategoryQuery.Where(x => x.Name.Trim() == data.CategoryName.Trim()).FirstAsync();
                refProduct.CategoryId = refCategory.Id;
                _Repository._DbContext.Products.Update(refProduct);
                return await Task.FromResult(string.Empty);
            });

            var doneOp = new Action(async () =>
            {
                await _Repository._DbContext.SaveChangesAsync();
            });
            return await _ImportRequest(file, importOp, doneOp);
        }
        #endregion

        #region ProductAndCategoryImportTemplate 导出根据CSV批量分类产品模版
        /// <summary>
        /// 导出根据CSV批量分类产品模版
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProductAndCategoryImportTemplate")]
        public IActionResult ProductAndCategoryImportTemplate()
        {
            return _ExportCSVTemplateRequest<ProductAndCategoryExportCSV>();
        }
        #endregion

        #region ExportData 导出产品基本信息
        /// <summary>
        /// 导出产品基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [Route("Export")]
        [HttpGet]
        public Task<IActionResult> ExportData([FromQuery] PagingRequestModel model, string categoryId = "", bool classify = true)
        {
            var advanceQuery = new Func<IQueryable<Product>, Task<IQueryable<Product>>>(async (query) =>
            {
                if (classify)
                {
                    if (!string.IsNullOrWhiteSpace(categoryId))
                    {
                        var curCategoryTree = await _Repository._DbContext.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == categoryId);
                        //如果是根节点,把所有取出,不做分类过滤
                        if (curCategoryTree != null && curCategoryTree.LValue > 1)
                        {
                            var categoryQ = from it in _Repository._DbContext.AssetCategoryTrees
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
                query = query.Where(x => x.ActiveFlag == AppConst.I_DataState_Active);
                return query;
            });

            var transMapping = new Func<ProductDTO, Task<ProductExportDataCSV>>(async (entity) =>
            {
                var csData = new ProductExportDataCSV();
                csData.ProductName = entity.Name;
                csData.CategoryName = entity.CategoryName;
                csData.Description = entity.Description;
                csData.CreatedTime = entity.CreatedTime.ToString("yyyy-MM-dd hh:mm:ss");
                csData.ModifiedTime = entity.ModifiedTime.ToString("yyyy-MM-dd hh:mm:ss");
                csData.Creator = entity.Creator;
                csData.Modifier = entity.Modifier;
                return await Task.FromResult(csData);
            });

            return _ExportDataRequest(model, transMapping);
        }

        #endregion

        #region  [ CSV Matedata ]
        class ProductAndCategoryImportCSV : ClassMap<ProductAndCategoryImportCSV>, ImportData
        {
            public ProductAndCategoryImportCSV()
                : base()
            {
                AutoMap();
            }
            public string ProductName { get; set; }
            public string CategoryName { get; set; }
            public string ErrorMsg { get; set; }
        }

        class ProductAndCategoryExportCSV : ClassMap<ProductAndCategoryImportCSV>
        {
            public ProductAndCategoryExportCSV()
                : base()
            {
                AutoMap();
            }

            public string ProductName { get; set; }
            public string CategoryName { get; set; }
        }

        class ProductExportDataCSV : ClassMap<ProductExportDataCSV>
        {
            public ProductExportDataCSV()
             : base()
            {
                AutoMap();
            }
            public string ProductName { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
            public string CreatedTime { get; set; }
            public string ModifiedTime { get; set; }
            public string Creator { get; set; }
            public string Modifier { get; set; }

        }
        #endregion
    }


}