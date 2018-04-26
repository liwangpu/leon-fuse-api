using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class SolutionStore : StoreBase<Solution, SolutionDTO>, IStore<Solution>
    {

        #region 构造函数
        public SolutionStore(ApiDbContext context)
        : base(context)
        {

        }
        #endregion

        /**************** public methods ****************/

        #region CanCreate 判断方案信息是否符合存储规范
        /// <summary>
        /// 判断方案信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanCreate(string accid, Solution data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "方案名称", 50));
            return errors;
        }
        #endregion

        #region CanUpdate 判断方案信息是否符合更新规范
        /// <summary>
        /// 判断方案信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanUpdate(string accid, Solution data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "方案名称", 50));
            return errors;
        }
        #endregion

        #region CanDelete 判断方案信息是否符合删除规范
        /// <summary>
        /// 判断方案信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanDelete(string accid, string id)
        {
            var errors = new List<string>();
            var valid = _CanDelete(accid, id);
            if (valid.Count > 0)
                return valid;
            return errors;
        }
        #endregion

        #region CanRead 判断用户是否符合读取权限
        /// <summary>
        /// 判断用户是否符合读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanRead(string accid, string id)
        {
            var errors = new List<string>();
            return errors;
        }
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
        public async Task<PagedData1<SolutionDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Solution, bool>> searchPredicate)
        {
            var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            var dtos = pagedData.Data.Select(x => x.ToDTO());
            return new PagedData1<SolutionDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SolutionDTO> GetByIdAsync(string accid, string id)
        {
            try
            {
                var data = await _GetByIdAsync(id);
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("GetByIdAsync", ex);
            }
            return new SolutionDTO();
        }
        #endregion

        #region SaveOrUpdateAsync 更新方案信息
        /// <summary>
        /// 更新方案信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveOrUpdateAsync(string accid, Solution data)
        {
            try
            {
                if (!data.IsPersistence())
                {
                    await _Repo.Context.Set<Solution>().AddAsync(data);
                }
                await _Repo.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("SaveOrUpdateAsync", ex);
            }
        }
        #endregion

        #region DeleteAsync 删除方案信息
        /// <summary>
        /// 删除方案信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string accid, string id)
        {
            try
            {
                var data = await _GetByIdAsync(id);
                if (data.IsPersistence())
                {
                    //var ps = _Repo.Context.Set<PermissionItem>().Where(x => x.ResId == data.Id).FirstOrDefault();
                    _Repo.Context.Set<Solution>().Remove(data);
                    //if (ps != null)
                    //    _Repo.Context.Set<PermissionItem>().Remove(ps);
                    await _Repo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("DeleteAsync", ex);
            }
        }
        #endregion

    }
}
