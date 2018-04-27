using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// ProductSpec Store
    /// </summary>
    public class ProductSpecStore : StoreBase<ProductSpec, ProductSpecDTO>, IStore<ProductSpec>
    {
        private readonly FileAssetStore _FileAssetStore;
        private readonly StaticMeshStore _StaticMeshStore;
        private readonly MaterialStore _MaterialStore;
        private readonly ProductStore _ProductStore;

        #region 构造函数
        public ProductSpecStore(ApiDbContext context)
        : base(context)
        {
            _FileAssetStore = new FileAssetStore(context);
            _StaticMeshStore = new StaticMeshStore(context);
            _MaterialStore = new MaterialStore(context);
            _ProductStore = new ProductStore(context);
        }
        #endregion

        /**************** public methods ****************/

        #region CanCreate 判断产品规格信息是否符合存储规范
        /// <summary>
        /// 判断产品规格信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanCreate(string accid, ProductSpec data)
        {
            var errors = new List<string>();

            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "规格名称", 50));
            if (string.IsNullOrWhiteSpace(data.ProductId))
                errors.Add(string.Format(ValidityMessage.V_RequiredRejectMsg, "产品编号"));
            var existProd = await _ProductStore._GetByIdAsync(data.ProductId);
            if (existProd == null)
                errors.Add(string.Format(ValidityMessage.V_NotReferenceMsg, "产品"));
            return errors;
        }
        #endregion

        #region CanUpdate 判断产品规格信息是否符合更新规范
        /// <summary> 
        /// 判断产品规格信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanUpdate(string accid, ProductSpec data)
        {
            var errors = new List<string>();

            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "规格名称", 50));
            if (string.IsNullOrWhiteSpace(data.ProductId))
                errors.Add(string.Format(ValidityMessage.V_RequiredRejectMsg, "产品编号"));
            var existProd = await _ProductStore._GetByIdAsync(data.ProductId);
            if (existProd == null)
                errors.Add(string.Format(ValidityMessage.V_NotReferenceMsg, "产品"));
            return errors;
        }
        #endregion

        #region CanDelete 判断产品信息是否符合删除规范
        /// <summary>
        /// 判断产品信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanDelete(string accid, string id)
        {
            var errors = new List<string>();

            var valid = _CanDelete(accid, id);
            if (valid.Count > 0)
                return valid;
            return errors;
        }
        #endregion

        #region CanRead 判断用户是否有读取权限
        /// <summary>
        /// 判断用户是否有读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanRead(string accid, string id)
        {
            var errors = new List<string>();
            return errors;
        }
        #endregion

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
        public async Task<PagedData1<ProductSpecDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<ProductSpec, bool>> searchPredicate)
        {
            var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            var dtos = pagedData.Data.Select(x => x.ToDTO());
            return new PagedData1<ProductSpecDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductSpecDTO> GetByIdAsync(string accid, string id)
        {
            try
            {
                var res = await _GetByIdAsync(id);
                if (!string.IsNullOrWhiteSpace(res.Icon))
                {
                    var ass = await _FileAssetStore._GetByIdAsync(res.Icon);
                    if (ass != null)
                        res.IconFileAsset = ass;
                }
                if (!string.IsNullOrWhiteSpace(res.CharletIds))
                {
                    var chartletIds = res.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                    {
                        var ass = await _FileAssetStore._GetByIdAsync(chartletIds[idx]);
                        if (ass != null)
                            res.CharletAsset.Add(ass);
                    }
                }
                if (!string.IsNullOrWhiteSpace(res.StaticMeshIds))
                {
                    var meshIds = res.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int idx = meshIds.Count - 1; idx >= 0; idx--)
                    {
                        var kv = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(meshIds[idx]);
                        var refMesh = await _StaticMeshStore._GetByIdAsync(kv.Key);
                        if (refMesh != null)
                        {
                            var tmp = await _FileAssetStore._GetByIdAsync(refMesh.FileAssetId);
                            if (tmp != null)
                                refMesh.FileAsset = tmp;
                        }

                        if (!string.IsNullOrWhiteSpace(kv.Value))
                        {
                            var matids = string.IsNullOrWhiteSpace(kv.Value) ? new List<string>() : kv.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in matids)
                            {
                                var refMat = await _MaterialStore._GetByIdAsync(item);
                                if (refMat != null)
                                {
                                    var tmp = await _FileAssetStore._GetByIdAsync(refMat.FileAssetId);
                                    if (tmp != null)
                                    {
                                        refMat.FileAsset = tmp;
                                        refMesh.Materials.Add(refMat);
                                    }

                                }
                            }
                        }
                        res.StaticMeshAsset.Add(refMesh);
                    }
                }
                return res.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("GetByIdAsync", ex);
            }
            return new ProductSpecDTO();
        }
        #endregion

        #region SaveOrUpdateAsync 更新产品规格信息
        /// <summary>
        /// 更新产品规格信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveOrUpdateAsync(string accid, ProductSpec data)
        {
            try
            {
                if (!data.IsPersistence())
                {
                    await _Repo.Context.Set<ProductSpec>().AddAsync(data);
                    //await _Repo.Context.Set<PermissionItem>().AddAsync(Permission.NewItem(accid, data.Id, "ProductSpec", PermissionType.All));
                }
                await _Repo.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("SaveOrUpdateAsync", ex);
            }
        }
        #endregion

        #region DeleteAsync 删除产品规格信息
        /// <summary>
        /// 删除产品规格信息
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
                    var ps = _Repo.Context.Set<PermissionItem>().Where(x => x.ResId == data.Id).FirstOrDefault();
                    _Repo.Context.Set<ProductSpec>().Remove(data);
                    if (ps != null)
                        _Repo.Context.Set<PermissionItem>().Remove(ps);
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
