using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ApiServer.Repositories
{
    public class PackageRepository : ResourceRepositoryBase<Package, PackageDTO>
    {
        protected IRepository<ProductReplaceGroup, ProductReplaceGroupDTO> _ProductReplaceGroupRep;
        public PackageRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep, IRepository<ProductReplaceGroup, ProductReplaceGroupDTO> replaceGroupRep)
            : base(context, permissionTreeRep)
        {
            _ProductReplaceGroupRep = replaceGroupRep;
        }

        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational;
            }
        }

        public override int ResType => ResourceTypeConst.Package;

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<PackageDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            if (!string.IsNullOrWhiteSpace(data.Content))
            {
                data.ContentIns = !string.IsNullOrWhiteSpace(data.Content) ? JsonConvert.DeserializeObject<PackageContent>(data.Content) : new PackageContent();
                #region 匹配套餐中内容项
                if (data.ContentIns.Items != null && data.ContentIns.Items.Count > 0)
                {
                    for (int idx = data.ContentIns.Items.Count - 1; idx >= 0; idx--)
                    {
                        var cur = data.ContentIns.Items[idx];
                        if (string.IsNullOrWhiteSpace(cur.ProductSpecId))
                            continue;

                        var spec = await _DbContext.ProductSpec.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == cur.ProductSpecId);
                        if (spec != null)
                        {
                            cur.ProductSpecName = spec.Name;
                            cur.ProductName = spec.Product != null ? spec.Product.Name : "";
                            data.ContentIns.Items[idx] = cur;
                        }
                    }

                }
                #endregion

                #region 匹配套餐中区域内容项
                if (data.ContentIns.Areas != null && data.ContentIns.Areas.Count > 0)
                {

                    var areas = data.ContentIns.Areas;
                    for (int kdx = areas.Count - 1; kdx >= 0; kdx--)
                    {
                        var curArea = areas[kdx];
                        if (string.IsNullOrWhiteSpace(curArea.AreaAlias))
                        {
                            var referArea = await _DbContext.AreaTypes.FirstOrDefaultAsync(x => x.Id == curArea.AreaTypeId);
                            if (referArea != null)
                                curArea.AreaAlias = referArea.Name;
                        }


                        #region 匹配产品组
                        if (curArea.GroupsMap != null && curArea.GroupsMap.Count > 0)
                        {
                            var groups = new List<ProductGroupDTO>();
                            foreach (var item in curArea.GroupsMap)
                            {
                                var grp = await _DbContext.ProductGroups.FindAsync(item.Value);
                                if (grp != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(grp.Icon))
                                        grp.IconFileAsset = await _DbContext.Files.FindAsync(grp.Icon);
                                    groups.Add(grp.ToDTO());
                                }

                            }
                            curArea.GroupsMapIns = groups;
                        }
                        #endregion

                        #region 匹配分类产品
                        if (curArea.ProductCategoryMap != null && curArea.ProductCategoryMap.Count > 0)
                        {
                            var products = new List<ProductDTO>();
                            foreach (var item in curArea.ProductCategoryMap)
                            {
                                var prd = await _DbContext.Products.FindAsync(item.Value);
                                if (prd != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(prd.Icon))
                                        prd.IconFileAsset = await _DbContext.Files.FindAsync(prd.Icon);
                                    products.Add(prd.ToDTO());
                                }
                            }
                            curArea.ProductCategoryMapIns = products;
                        }
                        #endregion

                        #region 匹配材质
                        if (curArea.Materials != null && curArea.Materials.Count > 0)
                        {
                            var materials = new List<PackageMaterial>();
                            foreach (var item in curArea.Materials)
                            {
                                var mtl = await _DbContext.Materials.FindAsync(item.Value);
                                if (mtl != null)
                                {
                                    var model = new PackageMaterial();
                                    if (!string.IsNullOrWhiteSpace(mtl.Icon))
                                    {
                                        var fs = await _DbContext.Files.FindAsync(mtl.Icon);
                                        model.Icon = fs != null ? fs.Url : "";
                                    }

                                    model.MaterialId = mtl.Id;
                                    model.LastActorName = item.Key;
                                    model.ActorName = item.Key;
                                    if (model.ActorName == "待定")
                                        materials.Insert(0, model);
                                    else
                                        materials.Add(model);
                                }
                            }
                            curArea.MaterialIns = materials;
                        }
                        #endregion
                    }
                    data.ContentIns.Areas = areas.OrderBy(x => x.AreaAlias).ToList();

                }
                #endregion

                #region 匹配套餐中的产品替换组
                if (data.ContentIns.ReplaceGroups != null && data.ContentIns.ReplaceGroups.Count > 0)
                {
                    var rpGroups = new List<ProductReplaceGroupDTO>();
                    foreach (var rpId in data.ContentIns.ReplaceGroups)
                    {
                        var rpDto = await _ProductReplaceGroupRep.GetByIdAsync(rpId);
                        if (rpDto != null)
                            rpGroups.Add(rpDto);
                    }
                    data.ContentIns.ReplaceGroupIns = rpGroups;
                }
                #endregion

            }

            if (!string.IsNullOrWhiteSpace(data.Icon))
            {
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            }

            return data.ToDTO();
        }
        #endregion

        #region _GetPermissionData
        ///// <summary>
        ///// _GetPermissionData
        ///// </summary>
        ///// <param name="accid"></param>
        ///// <param name="dataOp"></param>
        ///// <param name="withInActive"></param>
        ///// <returns></returns>
        //public override async Task<IQueryable<Package>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        //{
        //    var query = await base._GetPermissionData(accid, dataOp, withInActive);

        //    #region 获取父组织分享的方案数据
        //    if (dataOp == DataOperateEnum.Retrieve)
        //    {
        //        var account = await _DbContext.Accounts.FindAsync(accid);
        //        var curNode = await _DbContext.PermissionTrees.FirstAsync(x => x.ObjId == account.OrganizationId);
        //        var parentOrgQ = await _PermissionTreeRepository.GetAncestorNode(curNode, new List<string>() { AppConst.S_NodeType_Organization });
        //        var parentOrgIds = await parentOrgQ.Select(x => x.ObjId).ToListAsync();
        //        var shareDataQ = _DbContext.Packages.Where(x => parentOrgIds.Contains(x.OrganizationId) && x.ActiveFlag == AppConst.I_DataState_Active && x.ResourceType == (int)ResourceTypeEnum.Organizational_SubShare);
        //        query = query.Union(shareDataQ);
        //    }
        //    #endregion

        //    return query;
        //}
        #endregion

        #region PagedSelectExpression
        /// <summary>
        /// PagedSelectExpression
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<Package, Package>> PagedSelectExpression()
        {
            return x => new Package()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Description = x.Description,
                CategoryId = x.CategoryId,
                OrganizationId = x.OrganizationId,
                Creator = x.Creator,
                Modifier = x.Modifier,
                CreatedTime = x.CreatedTime,
                ModifiedTime = x.ModifiedTime,
                ResourceType = x.ResourceType
            };
        }
        #endregion
    }
}
