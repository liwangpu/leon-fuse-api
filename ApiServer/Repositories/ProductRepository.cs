using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiServer.Repositories
{
    public class ProductRepository : ResourceRepositoryBase<Product, ProductDTO>
    {
        public ProductRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }

        public override ResourceTypeEnum ResourceTypeSetting => ResourceTypeEnum.Organizational_SubShare;
        public override int ResType => ResourceTypeConst.Product;

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ProductDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            _DbContext.Entry(data).Collection(d => d.Specifications).Load();
            if (data.Specifications != null && data.Specifications.Count > 0)
            {
                for (int nidx = data.Specifications.Count - 1; nidx >= 0; nidx--)
                {
                    var spec = data.Specifications[nidx];
                    if (!string.IsNullOrWhiteSpace(spec.Icon))
                    {
                        var iconass = await _DbContext.Files.FindAsync(spec.Icon);
                        spec.IconFileAsset = iconass;
                    }

                    if (!string.IsNullOrWhiteSpace(spec.StaticMeshIds))
                    {
                        var map = JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
                        for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                        {
                            var refMesh = await _DbContext.StaticMeshs.FindAsync(map.Items[idx].StaticMeshId);
                            if (refMesh != null)
                            {
                                var tmp = await _DbContext.Files.FindAsync(refMesh.FileAssetId);
                                if (tmp != null)
                                    refMesh.FileAsset = tmp;
                            }

                            if (map.Items[idx].MaterialIds != null && map.Items[idx].MaterialIds.Count > 0)
                            {
                                var matids = map.Items[idx].MaterialIds;
                                foreach (var item in matids)
                                {
                                    var refMat = await _DbContext.Materials.FindAsync(item);
                                    if (refMat != null)
                                    {
                                        var tmp = await _DbContext.Files.FindAsync(refMat.FileAssetId);
                                        if (tmp != null)
                                        {
                                            refMat.FileAsset = tmp;
                                            refMesh.Materials.Add(refMat);
                                        }
                                    }
                                }
                            }
                            spec.StaticMeshAsset.Add(refMesh);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(data.Icon))
            {
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            }
            if (!string.IsNullOrWhiteSpace(data.CategoryId))
                data.AssetCategory = await _DbContext.AssetCategories.FindAsync(data.CategoryId);
            return data.ToDTO();
        }
        #endregion

        #region _GetPermissionData 获取权限数据
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="dataOp"></param>
        /// <param name="withInActive"></param>
        /// <returns></returns>
        public async override Task<IQueryable<Product>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = true)
        {
            IQueryable<Product> query;

            var currentAcc = await _DbContext.Accounts.Select(x => new Account() { Id = x.Id, OrganizationId = x.OrganizationId, Type = x.Type }).FirstOrDefaultAsync(x => x.Id == accid);

            //数据状态
            if (withInActive)
                query = _DbContext.Set<Product>();
            else
                query = _DbContext.Set<Product>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            //超级管理员系列不走权限判断
            if (currentAcc.Type == AppConst.AccountType_SysAdmin || currentAcc.Type == AppConst.AccountType_SysService)
                return await Task.FromResult(query);


            if (dataOp == DataOperateEnum.Retrieve)
            {
                /*
                 * 读取操作
                 *      管理员
                 *              品牌管理员/用户:获取自己组织的创建的产品
                 *       合伙人/供应商
                 *              管理员/用户:获取自己组织有读取权限的
                 */


                if (currentAcc.Type == AppConst.AccountType_BrandAdmin || currentAcc.Type == AppConst.AccountType_BrandMember)
                {
                    return query.Where(x => x.OrganizationId == currentAcc.OrganizationId);
                }
                else
                {
                    var permissionIdQ = _DbContext.ResourcePermissions.Where(x => x.OrganizationId == currentAcc.OrganizationId && x.ResType == ResType && x.OpRetrieve == 1);

                    query = from it in query
                            join ps in permissionIdQ on it.Id equals ps.ResId
                            select it;
                    return query;
                }
            }
            else
            {
                if (currentAcc.Type == AppConst.AccountType_BrandAdmin)
                {
                    return query.Where(x => x.OrganizationId == currentAcc.OrganizationId);
                }
                if (currentAcc.Type == AppConst.AccountType_BrandMember)
                {
                    return query.Where(x => x.Creator == currentAcc.Id);
                }
            }

            return query.Take(0);
        }

        #endregion

        #region override SimplePagedQueryAsync
        /// <summary>
        /// SimplePagedQueryAsync
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accid"></param>
        /// <param name="advanceQuery"></param>
        /// <returns></returns>
        public override async Task<PagedData<Product>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<Product>, Task<IQueryable<Product>>> advanceQuery = null)
        {
            var result = await base.SimplePagedQueryAsync(model, accid, advanceQuery);

            if (result.Total > 0)
            {
                for (int idx = result.Data.Count - 1; idx >= 0; idx--)
                {
                    var curData = result.Data[idx];
                    if (!string.IsNullOrWhiteSpace(curData.Icon))
                        curData.IconFileAsset = await _DbContext.Files.FindAsync(curData.Icon);

                    if (!string.IsNullOrWhiteSpace(curData.CategoryId))
                        curData.AssetCategory = await _DbContext.AssetCategories.FindAsync(curData.CategoryId);

                    var defaultSpec = await _DbContext.ProductSpec.Where(x => x.ProductId == curData.Id && x.ActiveFlag == AppConst.I_DataState_Active).OrderByDescending(x => x.CreatedTime).FirstOrDefaultAsync();
                    if (defaultSpec != null)
                        curData.Specifications = new List<ProductSpec>() { defaultSpec };
                }
            }
            return result;
        }
        #endregion

        public override async Task UpdateAsync(string accid, Product data)
        {
            await base.UpdateAsync(accid, data);
            //更新默认第一个规格的icon
            var productSpec = await _DbContext.ProductSpec.Where(x => x.Product == data).FirstOrDefaultAsync();
            if (productSpec != null)
            {
                productSpec.Icon = data.Icon;
                _DbContext.ProductSpec.Update(productSpec);
                await _DbContext.SaveChangesAsync();
            }
        }

        #region GetSpecByStaticMesh 根据产品Id和模型id获取该产品中模型为输入模型id的所有产品规格列表
        /// <summary>
        /// 根据产品Id和模型id获取该产品中模型为输入模型id的所有产品规格列表
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="staticMeshId"></param>
        /// <returns></returns>
        public async Task<List<ProductSpec>> GetSpecByStaticMesh(string productId, string staticMeshId)
        {
            var specs = new List<ProductSpec>();
            var product = await _DbContext.Products.Include(x => x.Specifications).FirstOrDefaultAsync(x => x.Id == productId);
            if (product != null)
            {
                foreach (var spec in product.Specifications)
                {
                    var map = !string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds) : new SpecMeshMap();
                    if (map.Items.Count(x => x.StaticMeshId == staticMeshId) > 0)
                        specs.Add(spec);
                }
            }
            return specs;
        }
        #endregion
    }
}
