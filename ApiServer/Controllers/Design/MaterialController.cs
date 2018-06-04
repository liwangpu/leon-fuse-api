using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class MaterialController : ListableController<Material, MaterialDTO>
    {
        private readonly ApiDbContext _context;

        #region 构造函数
        public MaterialController(ApiDbContext context)
        : base(new MaterialStore(context))
        {
            _context = context;
        }
        #endregion

        #region Get 根据分页查询信息获取材质概要信息
        /// <summary>
        /// 根据分页查询信息获取材质概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<MaterialDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "", bool classify = true)
        {
            var advanceQuery = new Func<IQueryable<Material>, Task<IQueryable<Material>>>(async (query) =>
            {
                if (classify)
                {
                    if (!string.IsNullOrWhiteSpace(categoryId))
                    {
                        var curCategoryTree = await _context.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == categoryId);
                        //如果是根节点,把所有取出,不做分类过滤
                        if (curCategoryTree != null && curCategoryTree.LValue > 1)
                        {
                            var categoryQ = from it in _context.AssetCategoryTrees
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
                return await Task.FromResult(query);
            });

            return await _GetPagingRequest(model, null, advanceQuery);
        }
        #endregion

        #region Get 根据id获取材质信息
        /// <summary>
        /// 根据id获取材质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 信息材质信息
        /// <summary>
        /// 信息材质信息
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
                entity.CategoryId = model.CategoryId;
                entity.Dependencies = model.Dependencies;
                entity.Parameters = model.Parameters;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
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
        public async Task<IActionResult> Put([FromBody]MaterialEditModel model)
        {
            var mapping = new Func<Material, Task<Material>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.PackageName = model.PackageName;
                entity.FileAssetId = model.FileAssetId;
                entity.CategoryId = model.CategoryId;
                entity.Dependencies = model.Dependencies;
                entity.Parameters = model.Parameters;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region BulkChangeCategory 批量修改材质分类信息
        /// <summary>
        /// 批量修改材质分类信息
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
            var existCategory = await _context.AssetCategories.CountAsync(x => x.Id == model.CategoryId) > 0;
            if (!existCategory)
            {
                ModelState.AddModelError("categoryId", "对应记录不存在");
                return new ValidationFailedResult(ModelState);
            }
            var idArr = model.Ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    for (int idx = idArr.Length - 1; idx >= 0; idx--)
                    {
                        var id = idArr[idx];
                        var refMaterial = await _context.Materials.FindAsync(id);
                        if (refMaterial != null)
                        {
                            refMaterial.CategoryId = model.CategoryId;
                            _context.Materials.Update(refMaterial);
                        }
                        else
                        {
                            ModelState.AddModelError("ProductId", "对应记录不存在");
                            return new ValidationFailedResult(ModelState);
                        }
                    }
                    _context.SaveChanges();
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

        #region ImportMaterialAndCategory 根据CSV批量分类材质
        /// <summary>
        /// 根据CSV批量分类材质
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("ImportMaterialAndCategory")]
        [HttpPut]
        public async Task<IActionResult> ImportMaterialAndCategory(IFormFile file)
        {
            var accid = AuthMan.GetAccountId(this);
            var currentAcc = await _context.Accounts.FindAsync(accid);
            var psMaterialQuery = await _Store._GetPermissionData(accid, DataOperateEnum.Update);
            var psCategoryQuery = _context.AssetCategories.Where(x => x.Type == AppConst.S_Category_Material && x.ActiveFlag == AppConst.I_DataState_Active && x.OrganizationId == currentAcc.OrganizationId);
            var importOp = new Func<MaterialAndCategoryImportCSV, Task<string>>(async (data) =>
            {
                var mapProductCount = await psMaterialQuery.Where(x => x.Name.Trim() == data.MaterialName.Trim()).CountAsync();
                if (mapProductCount == 0)
                    return "没有找到该材质或您没有权限修改该条数据";
                if (mapProductCount > 1)
                    return "材质名称有重复,请手动分配该材质";
                var mapCategoryCount = await psCategoryQuery.Where(x => x.Name == data.CategoryName).CountAsync();
                if (mapCategoryCount == 0)
                    return "没有找到该分类,请确认分类名称是否有误";
                if (mapCategoryCount > 1)
                    return "分类名称有重复,请手动分配该材质";

                var refProduct = await psMaterialQuery.Where(x => x.Name.Trim() == data.MaterialName.Trim()).FirstAsync();
                var refCategory = await psCategoryQuery.Where(x => x.Name.Trim() == data.CategoryName.Trim()).FirstAsync();
                refProduct.CategoryId = refCategory.Id;
                _context.Materials.Update(refProduct);
                return await Task.FromResult(string.Empty);
            });

            var doneOp = new Action(async () =>
            {
                await _context.SaveChangesAsync();
            });
            return await _ImportRequest(file, importOp, doneOp);
        }
        #endregion

        #region MaterialAndCategoryImportTemplate 导出根据CSV批量分类材质模版
        /// <summary>
        /// 导出根据CSV批量分类材质模版
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MaterialAndCategoryImportTemplate")]
        public IActionResult MaterialAndCategoryImportTemplate()
        {
            return _ExportCSVTemplateRequest<MaterialAndCategoryExportCSV>();
        }
        #endregion

        #region  [ CSV Matedata ]
        class MaterialAndCategoryImportCSV : ClassMap<MaterialAndCategoryImportCSV>, ImportData
        {
            public MaterialAndCategoryImportCSV()
                : base()
            {
                AutoMap();
            }
            public string MaterialName { get; set; }
            public string CategoryName { get; set; }
            public string ErrorMsg { get; set; }
        }

        class MaterialAndCategoryExportCSV : ClassMap<MaterialAndCategoryExportCSV>
        {
            public MaterialAndCategoryExportCSV()
                : base()
            {
                AutoMap();
            }

            public string MaterialName { get; set; }
            public string CategoryName { get; set; }
        }
        #endregion
    }
}