using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
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
    }
}