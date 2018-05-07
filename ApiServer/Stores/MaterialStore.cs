using ApiModel.Entities;
using ApiServer.Data;
using BambooCore;
using System;
using System.Linq.Expressions;
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

        #region SimpleQueryAsync 简单返回分页查询DTO信息
        /// <summary>
        /// 简单返回分页查询DTO信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchPredicate"></param>
        /// <returns></returns>
        public async Task<PagedData<MaterialDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Material, bool>> searchPredicate)
        {
            //var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            //var dtos = pagedData.Data.Select(x => x.ToDTO());
            //return new PagedData<MaterialDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
            //TODO:
            return new PagedData<MaterialDTO>();
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaterialDTO> GetByIdAsync(string accid, string id)
        {
            var data = await _GetByIdAsync( id);
            return data.ToDTO();
        }
        #endregion
    }
}
