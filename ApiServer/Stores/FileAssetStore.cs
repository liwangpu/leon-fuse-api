using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class FileAssetStore : StoreBase<FileAsset, FileAssetDTO>, IStore<FileAsset, FileAssetDTO>
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
        public FileAssetStore(ApiDbContext context)
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
        public async Task SatisfyCreateAsync(string accid, FileAsset data, ModelStateDictionary modelState)
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
        public async Task SatisfyUpdateAsync(string accid, FileAsset data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion
    }
}
