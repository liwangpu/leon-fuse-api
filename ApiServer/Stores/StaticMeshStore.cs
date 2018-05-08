﻿using ApiModel.Entities;
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
    /// <summary>
    /// StaticMesh Store
    /// </summary>
    public class StaticMeshStore : StoreBase<StaticMesh>, IStore<StaticMesh>
    {
        #region 构造函数
        public StaticMeshStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region CanCreate 判断模型文件信息是否符合存储规范
        /// <summary>
        /// 判断模型文件信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanCreate(string accid, StaticMesh data)
        {
            var valid = _CanSave(accid, data);
            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "模型名称", 50);
            if (string.IsNullOrWhiteSpace(data.FileAssetId))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "关联文件");

            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断模型文件信息是否符合更新规范
        /// <summary>
        /// 判断模型文件信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, StaticMesh data)
        {
            var valid = _CanSave(accid, data);
            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "模型名称", 50);
            if (string.IsNullOrWhiteSpace(data.FileAssetId))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "关联文件");

            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断模型信息信息是否符合删除规范
        /// <summary>
        /// 判断模型信息信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanDelete(string accid, string id)
        {
            var errors = new List<string>();

            var valid = _CanDelete(accid, id);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanRead 判断用户是否有读取权限
        /// <summary>
        /// 判断用户是否有读取权限
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
        public async Task<PagedData<StaticMeshDTO>> SimpleQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<StaticMesh, bool>> searchPredicate)
        {
            //var pagedData = await _SimplePagedQueryAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
            //var dtos = pagedData.Data.Select(x => x.ToDTO());
            //return new PagedData<StaticMeshDTO>() { Data = pagedData.Data.Select(x => x.ToDTO()), Page = pagedData.Page, Size = pagedData.Size, Total = pagedData.Total };

            //TODO:
            return new PagedData<StaticMeshDTO>();
        }
        #endregion

        #region SaveOrUpdateAsync 更新模型信息
        /// <summary>
        /// 更新模型信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveOrUpdateAsync(string accid, StaticMesh data)
        {
            try
            {
                if (!data.IsPersistence())
                {
                    await _DbContext.Set<StaticMesh>().AddAsync(data);
                    //await _Repo.Context.Set<PermissionItem>().AddAsync(Permission.NewItem(accid, data.Id, "Product", PermissionType.All));
                }
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("SaveOrUpdateAsync", ex);
            }
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StaticMeshDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            return data.ToDTO();
        }


        #endregion

        #region DeleteAsync 删除产品信息
        /// <summary>
        /// 删除产品信息
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
                    _DbContext.Set<StaticMesh>().Remove(data);
                    //if (ps != null)
                    //    _Repo.Context.Set<PermissionItem>().Remove(ps);
                    await _DbContext.SaveChangesAsync();
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
