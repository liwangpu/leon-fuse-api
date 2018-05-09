using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// Material Store
    /// </summary>
    public class MaterialStore : StoreBase<Material>
    {
        #region 构造函数
        public MaterialStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreate 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreate(string accid, Material data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region SatisfyUpdate 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdate(string accid, Material data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion
    }
}
