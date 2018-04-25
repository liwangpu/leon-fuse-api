using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    /// <summary>
    /// Product Store
    /// </summary>
    public class ProductStore : StoreBase<Product, ProductDTO>
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

        #region CanSave 判断产品信息是否符合存储规范
        /// <summary>
        /// 判断产品信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<List<string>> CanSave(string accid, Product data)
        {
            var errors = new List<string>();
            //基类做数据为空或者权限验证
            var valid = await _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;
            //派生类做数据有效性验证
            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "产品名称", 50));
            return errors;
        }
        #endregion

        #region CanDelete 判断产品信息是否符合删除规范
        /// <summary>
        /// 判断产品信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<List<string>> CanDelete(string accid, Product data)
        {
            var errors = new List<string>();
            //基类做数据为空或者权限验证
            var valid = await _CanDelete(accid, data);
            if (valid.Count > 0)
                return valid;
            return errors;
        }
        #endregion

        /**************** public methods ****************/

        #region SimpleQueryAsync 简单返回分页查询DTO信息
        /// <summary>
        /// 简单返回分页查询DTO信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchPredicate"></param>
        /// <returns></returns>
        public async Task<PagedData1<ProductDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Product, bool>> searchPredicate)
        {
            var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            var dtos = pagedData.Data.Select(x => x.ToDTO());
            return new PagedData1<ProductDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
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
            var data = await _GetByIdAsync(accid, id);
            _Repo.Context.Entry(data).Collection(d => d.Specifications).Load();
            if (data.Specifications != null && data.Specifications.Count > 0)
            {
                for (int nidx = data.Specifications.Count - 1; nidx >= 0; nidx--)
                {
                    var spec = data.Specifications[nidx];
                    if (!string.IsNullOrWhiteSpace(spec.Icon))
                    {
                        var iconass = await _FileAssetStore._GetByIdAsync(accid, spec.Icon);
                        spec.IconFileAsset = iconass;
                    }
                }
            }
            return data.ToDTO();
        }
        #endregion

        #region SaveOrUpdateAsync 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> SaveOrUpdateAsync(string accid, Product data)
        {
            var valid = await CanSave(accid, data);
            if (valid.Count == 0)
            {
                if (!data.IsPersistence())
                {
                    await _Repo.Context.Set<Product>().AddAsync(data);
                    await _Repo.Context.Set<PermissionItem>().AddAsync(Permission.NewItem(accid, data.Id, "Product", PermissionType.All));
                }
                await _Repo.Context.SaveChangesAsync();
            }
            return valid;
        }
        #endregion

        #region DeleteAsync 删除产品信息
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> DeleteAsync(string accid, string id)
        {
            var data = await _GetByIdAsync(accid, id);
            var valid = await CanDelete(accid, data);
            if (valid.Count == 0)
            {
                if (data.IsPersistence())
                {
                    var ps = _Repo.Context.Set<PermissionItem>().Where(x => x.ResId == data.Id).FirstOrDefault();
                    _Repo.Context.Set<Product>().Remove(data);
                    if (ps != null)
                        _Repo.Context.Set<PermissionItem>().Remove(ps);
                    await _Repo.SaveChangesAsync();
                }
            }
            return valid;
        }
        #endregion
    }
}
