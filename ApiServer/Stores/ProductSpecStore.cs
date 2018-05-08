using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// ProductSpec Store
    /// </summary>
    public class ProductSpecStore : PermissionStore<ProductSpec>
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
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanCreate(string accid, ProductSpec data, ModelStateDictionary modelState)
        {

            await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断产品规格信息是否符合更新规范
        /// <summary>
        /// 判断产品规格信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task CanUpdate(string accid, ProductSpec data, ModelStateDictionary modelState)
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
            var query = from it in _DbContext.ProductSpec
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region CanRead 判断用户是否有读取权限
        /// <summary>
        /// 判断用户是否有读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CanRead(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.ProductSpec
                        select it;
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
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
        public async Task<PagedData<ProductSpecDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<ProductSpec, bool>> searchPredicate)
        {
            //var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            //var dtos = pagedData.Data.Select(x => x.ToDTO());
            //return new PagedData<ProductSpecDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };

            //TODO:
            return new PagedData<ProductSpecDTO>();
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
                    var map = JsonConvert.DeserializeObject<SpecMeshMap>(res.StaticMeshIds);
                    for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                    {
                        var refMesh = await _StaticMeshStore._GetByIdAsync(map.Items[idx].StaticMeshId);
                        if (refMesh != null)
                        {
                            var tmp = await _FileAssetStore._GetByIdAsync(refMesh.FileAssetId);
                            if (tmp != null)
                                refMesh.FileAsset = tmp;
                        }

                        if (map.Items[idx].MaterialIds != null && map.Items[idx].MaterialIds.Count > 0)
                        {
                            var matids = map.Items[idx].MaterialIds;
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

        #region CreateAsync 新建规格信息
        /// <summary>
        /// 新建规格信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProductSpecDTO> CreateAsync(string accid, ProductSpec data)
        {
            try
            {
                data.Id = GuidGen.NewGUID();
                data.Creator = accid;
                data.Modifier = accid;
                data.CreatedTime = DateTime.Now;
                data.ModifiedTime = DateTime.Now;
                _DbContext.ProductSpec.Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductSpecStore CreateAsync", ex);
                return new ProductSpecDTO();
            }
            return data.ToDTO();
        }
        #endregion

        #region UpdateAsync 更新规格信息
        /// <summary>
        /// 更新规格信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ProductSpecDTO> UpdateAsync(string accid, ProductSpec data)
        {
            try
            {
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.ProductSpec.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductSpecStore UpdateAsync", ex);
            }
            return new ProductSpecDTO();
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
