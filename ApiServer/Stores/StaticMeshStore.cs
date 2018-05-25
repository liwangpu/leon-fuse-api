using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class StaticMeshStore : ListableStore<StaticMesh, StaticMeshDTO>, IStore<StaticMesh, StaticMeshDTO>
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
        public StaticMeshStore(ApiDbContext context)
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
        public async Task SatisfyCreateAsync(string accid, StaticMesh data, ModelStateDictionary modelState)
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
        public async Task SatisfyUpdateAsync(string accid, StaticMesh data, ModelStateDictionary modelState)
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
        public override async Task<StaticMeshDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(data.Icon))
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            if (!string.IsNullOrWhiteSpace(data.FileAssetId))
                data.FileAsset = await _DbContext.Files.FindAsync(data.Icon);
            return data.ToDTO();
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
        public override async Task<PagedData<StaticMesh>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<StaticMesh>, Task<IQueryable<StaticMesh>>> advanceQuery = null)
        {
            var result = await base.SimplePagedQueryAsync(model, accid, advanceQuery);

            if (result.Total > 0)
            {
                for (int idx = result.Data.Count - 1; idx >= 0; idx--)
                {
                    var curData = result.Data[idx];
                    if (!string.IsNullOrWhiteSpace(curData.Icon))
                        curData.IconFileAsset = await _DbContext.Files.FindAsync(curData.Icon);
                }
            }
            return result;
        }
        #endregion
    }
}
