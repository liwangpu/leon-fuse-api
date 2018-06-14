using ApiModel;
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
                                var grp = await _DbContext.ProductGroups.FindAsync(item.Key);
                                if (grp != null && !string.IsNullOrWhiteSpace(grp.Icon))
                                {
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
