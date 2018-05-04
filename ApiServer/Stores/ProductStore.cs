using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    /// <summary>
    /// Product Store
    /// </summary>
    public class ProductStore : StoreBase<Product>, IStore<Product>
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
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public async Task<PagedData<ProductDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Product, bool>> searchExpression)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Products
                            select it;
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
        /// <returns></returns>
        public async Task<string> CanCreate(string accid, Product data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "产品名称", 50);
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断产品信息是否符合更新规范
        /// <summary>
        /// 判断产品信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, Product data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "产品名称", 50);
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断产品信息是否符合删除规范
        /// <summary>
        /// 判断产品信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanDelete(string accid, string id)
        {
            var valid = _CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanRead 判断用户是否符合读取权限
        /// <summary>
        /// 判断用户是否符合读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanRead(string accid, string id)
        {
            return await Task.FromResult(string.Empty);
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

        #region SaveOrUpdateAsync 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveOrUpdateAsync(string accid, Product data)
        {
            try
            {
                if (!data.IsPersistence())
                {
                    await _DbContext.Set<Product>().AddAsync(data);
                    //await _Repo.Context.Set<PermissionItem>().AddAsync(Permission.NewItem(accid, data.Id, "Product", PermissionType.All));
                }
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("SaveOrUpdateAsync", ex);
            }
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
