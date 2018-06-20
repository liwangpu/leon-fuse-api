using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class PackageStore : ListableStore<Package, PackageDTO>, IStore<Package, PackageDTO>
    {
        /// <summary>
        /// 资源访问类型
        /// </summary>
        public override ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Organizational;
            }
        }

        #region 构造函数
        public PackageStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Package data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, Package data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

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

                        #region 匹配替换组
                        if (curArea.ReplaceGroups != null && curArea.ReplaceGroups.Count > 0)
                        {
                            var groups = new List<List<ProductDTO>>();
                            foreach (var item in curArea.ReplaceGroups)
                            {
                                var replaceGroupItem = new List<ProductDTO>();
                                if (item.Products != null && item.Products.Count > 0)
                                {
                                    foreach (var productId in item.Products)
                                    {
                                        var prd = await DbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
                                        if (prd != null)
                                        {
                                            if (!string.IsNullOrWhiteSpace(prd.Icon))
                                                prd.IconFileAsset = await DbContext.Files.FirstOrDefaultAsync(x => x.Id == prd.Icon);

                                            if (prd.Id == item.DefaultId)
                                                replaceGroupItem.Insert(0, prd.ToDTO());
                                            else
                                                replaceGroupItem.Add(prd.ToDTO());
                                        }
                                    }
                                }
                                groups.Add(replaceGroupItem);
                            }
                            curArea.ReplaceGroupIns = groups;
                        }
                        #endregion
                    }

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
    }
}
