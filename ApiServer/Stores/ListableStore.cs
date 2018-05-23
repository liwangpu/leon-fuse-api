using ApiModel;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class ListableStore<T, DTO> : StoreBase<T, DTO>
     where T : class, IListable, IDTOTransfer<DTO>, new()
    where DTO : class, IData, new()
    {

        #region 构造函数
        public ListableStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region override GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<DTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(data.Icon))
            {
                data.IconFileAsset = await _DbContext.Files.FindAsync(data.Icon);
            }
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
        public override async Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null)
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
