using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    /// <summary>
    /// Product Store
    /// </summary>
    public class ProductStore : PermissionStore<Product>
    {
        private readonly FileAssetStore _FileAssetStore;

        #region 构造函数
        public ProductStore(ApiDbContext context)
        : base(context)
        {
            _FileAssetStore = new FileAssetStore(context);
        }
        #endregion

        /**************** protected methods ****************/

        #region CategoryPipe 分类过滤管道
        /// <summary>
        /// 分类过滤管道
        /// </summary>
        /// <param name="query"></param>
        /// <param name="categoryNode"></param>
        protected void CategoryPipe(ref IQueryable<Product> query, AssetCategoryTree categoryNode)
        {
            if (categoryNode != null)
            {
                var categoryQ = from cat in _DbContext.AssetCategoryTrees
                                where cat.LValue >= categoryNode.LValue && cat.RValue <= categoryNode.RValue
                                && cat.OrganizationId == categoryNode.OrganizationId
                                select cat;
                query = from it in query
                        join cat in categoryQ on it.CategoryId equals cat.ObjId
                        select it;
            }
        } 
        #endregion

        /**************** public methods ****************/

        #region SimplePagedQueryAsync 简单返回分页查询DTO信息
        /// <summary>
        /// 简单返回分页查询DTO信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="categoryId"></param>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public async Task<PagedData<ProductDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, string categoryId, Expression<Func<Product, bool>> searchExpression = null)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var currentCatNode = await _DbContext.AssetCategoryTrees.Where(x => x.ObjId == categoryId).FirstOrDefaultAsync();
                var query = from it in _DbContext.Products
                            select it;
                CategoryPipe(ref query, currentCatNode);
                _OrderByPipe(ref query, orderBy, desc);
                _SearchExpressionPipe(ref query, searchExpression);
                _BasicPermissionPipe(ref query, currentAcc);
                var result = await query.SimplePaging(page, pageSize);
                if (result.Total > 0)
                    return new PagedData<ProductDTO>() { Data = result.Data.Select(x => x.ToDTO()), Total = result.Total, Page = page, Size = pageSize };
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore SimplePagedQueryAsync", ex);
            }
            return new PagedData<ProductDTO>();
        }
        #endregion

        #region CanCreate 判断产品信息是否符合存储规范
        /// <summary>
        /// 判断产品信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanCreate(string accid, Product data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断产品信息是否符合更新规范
        /// <summary>
        /// 判断产品信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanUpdate(string accid, Product data, ModelStateDictionary modelState)
        {

            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断产品信息是否符合删除规范
        /// <summary>
        /// 判断产品信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanDelete(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Products
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region CanRead 判断用户是否符合读取权限
        /// <summary>
        /// 判断用户是否符合读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanRead(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Products
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductDTO> GetByIdAsync(string accid, string id)
        {
            try
            {
                var data = await _GetByIdAsync(id);
                _Repo.Context.Entry(data).Collection(d => d.Specifications).Load();
                if (data.Specifications != null && data.Specifications.Count > 0)
                {
                    for (int nidx = data.Specifications.Count - 1; nidx >= 0; nidx--)
                    {
                        var spec = data.Specifications[nidx];
                        if (!string.IsNullOrWhiteSpace(spec.Icon))
                        {
                            var iconass = await _FileAssetStore._GetByIdAsync(spec.Icon);
                            spec.IconFileAsset = iconass;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(data.CategoryId))
                    data.AssetCategory = await _DbContext.AssetCategories.FindAsync(data.CategoryId);
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("GetByIdAsync", ex);
            }
            return new ProductDTO();
        }
        #endregion

        #region CreateAsync 新建产品信息
        /// <summary>
        /// 新建产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProductDTO> CreateAsync(string accid, Product data)
        {
            try
            {
                data.Id = GuidGen.NewGUID();
                data.Creator = accid;
                data.Modifier = accid;
                data.CreatedTime = DateTime.Now;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Products.Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore CreateAsync", ex);
                return new ProductDTO();
            }
            return data.ToDTO();
        }
        #endregion

        #region UpdateAsync 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProductDTO> UpdateAsync(string accid, Product data)
        {
            try
            {
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Products.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore UpdateAsync", ex);
            }
            return new ProductDTO();
        }
        #endregion

        #region DeleteAsync 删除产品信息
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string accid, string id)
        {
            try
            {
                var data = await _GetByIdAsync(id);
                if (data.IsPersistence())
                {
                    //var ps = _Repo.Context.Set<PermissionItem>().Where(x => x.ResId == data.Id).FirstOrDefault();
                    _Repo.Context.Set<Product>().Remove(data);
                    //if (ps != null)
                    //    _Repo.Context.Set<PermissionItem>().Remove(ps);
                    await _Repo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("DeleteAsync", ex);
            }
        }
        #endregion
    }
}
