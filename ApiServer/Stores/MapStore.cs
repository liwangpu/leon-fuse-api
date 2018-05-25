using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class MapStore : ListableStore<Map, MapDTO>, IStore<Map, MapDTO>
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
        public MapStore(ApiDbContext context)
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
        public async Task SatisfyCreateAsync(string accid, Map data, ModelStateDictionary modelState)
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
        public async Task SatisfyUpdateAsync(string accid, Map data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<MapDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(data.Icon))
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            if (!string.IsNullOrWhiteSpace(data.FileAssetId))
                data.FileAsset = await _DbContext.Files.FindAsync(data.Icon);
            return data.ToDTO();
        }
        #endregion
    }
}
