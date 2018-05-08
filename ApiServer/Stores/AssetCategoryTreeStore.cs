using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Stores
{
    public class AssetCategoryTreeStore : TreeStore<AssetCategoryTree>
    {
        #region 构造函数
        public AssetCategoryTreeStore(ApiDbContext context)
           : base(context)
        {

        }
        #endregion

        #region GetFlatCategory 获取扁平结构的类别信息
        /// <summary>
        /// 获取扁平结构的类别信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<List<AssetCategoryDTO>> GetFlatCategory(string type, string organId)
        {
            try
            {
                var categoryQ = from cat in _DbContext.AssetCategories
                                where cat.OrganizationId == organId && cat.Type == type
                                select cat;
                return await categoryQ.Select(x => x.ToDTO()).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("AssetCategoryTreeStore GetFlatCategory", ex);
            }
            return new List<AssetCategoryDTO>();
        }
        #endregion
    }
}
