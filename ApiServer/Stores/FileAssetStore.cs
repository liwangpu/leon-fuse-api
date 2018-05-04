using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// FileAsset Store
    /// </summary>
    public class FileAssetStore : StoreBase<FileAsset>, IStore<FileAsset>
    {
        #region 构造函数
        public FileAssetStore(ApiDbContext context)
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
        public async Task<PagedData<FileAssetDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<FileAsset, bool>> searchPredicate)
        {
            var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            var dtos = pagedData.Data.Select(x => x.ToDTO());
            return new PagedData<FileAssetDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileAssetDTO> GetByIdAsync(string accid, string id)
        {
            var data = await _GetByIdAsync(id);
            return data.ToDTO();
        }
        #endregion

        /// <summary>
        /// 判断资源信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanCreate(string accid, FileAsset data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "文件名称", 50);

            return await Task.FromResult(string.Empty);
        }

        /// <summary>
        /// 判断资源信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, FileAsset data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "文件名称", 50);

            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDelete(string accid, string id)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanRead(string accid, string id)
        {
            return await Task.FromResult(string.Empty);
        }

        #region UpdateAsync 更新资源信息
        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<FileAssetDTO> UpdateAsync(string accid, FileAsset data)
        {
            try
            {
                _DbContext.Files.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("AccountStore UpdateAsync", ex);
            }
            return new FileAssetDTO();
        }
        #endregion

    }
}
