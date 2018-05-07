﻿using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class SolutionStore : StoreBase<Solution>, IStore<Solution>
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
        public async Task<string> CanCreate(string accid, Solution data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "方案名称", 50);
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断方案信息是否符合更新规范
        /// <summary>
        /// 判断方案信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, Solution data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "方案名称", 50);
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断方案信息是否符合删除规范
        /// <summary>
        /// 判断方案信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanDelete(string accid, string id)
        {
            var valid = _CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanRead 判断用户是否符合读取权限
        /// <summary>
        /// 判断用户是否符合读取权限
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanRead(string accid, string id)
        {
            return await Task.FromResult(string.Empty);
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
        public async Task<PagedData<SolutionDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Solution, bool>> searchPredicate)
        {
            //var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            //var dtos = pagedData.Data.Select(x => x.ToDTO());
            //return new PagedData<SolutionDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };
            //TODO:
            return new PagedData<SolutionDTO>();
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
