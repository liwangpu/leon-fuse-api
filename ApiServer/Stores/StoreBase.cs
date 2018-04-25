using ApiModel;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// StoreBase是所有store仓库类的基类
    /// StoreBase是业务无关的,不关注任何业务信息
    /// StoreBase是DTO无关的,返回的信息只是最原始的实体数据信息
    /// 如果需要返回DTO数据,请在派生Store类里面实现
    /// StoreBase应该是权限无关的,它只做简单操作,如果需要对权限做操作,请在派生类里面实现,但为了便利,还是在基类提供了一些和权限相关的操作,比如查询,CanSave...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class StoreBase<T, U>
         where T : class, IEntity, ApiModel.ICloneable, IDTOTransfer<U>, new()
        where U : IData
    {
        protected readonly Repository1<T> _Repo;
        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _Repo = new Repository1<T>(context);
        }
        #endregion

        /**************** protected method ****************/

        #region _SimplePagedQueryAsync 根据查询参数获取数据信息
        /// <summary>
        /// 根据查询参数获取数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchPredicate"></param>
        /// <returns></returns>
        protected async Task<PagedData1<T>> _SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<T, bool>> searchPredicate)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = SiteConfig.Instance.Json.DefaultPageSize;
            return await _Repo.GetAsync(accid, page, pageSize, orderBy, desc, searchPredicate);
        }
        #endregion

        #region _CanReadWidthAuthAsync 是否有权限查看
        /// <summary>
        /// 是否有权限查看
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<bool> _CanReadWidthAuthAsync(string accid, string id)
        {
            return await _Repo.CanReadAsync(accid, id);
        }
        #endregion

        #region _CanUpdateWidthAuthAsync 是否有权限更新
        /// <summary>
        /// 是否有权限更新
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<bool> _CanUpdateWidthAuthAsync(string accid, string id)
        {
            return await _Repo.CanUpdateAsync(accid, id);
        }
        #endregion

        #region _CanUpdateWidthAuthAsync 是否有权限更新
        /// <summary>
        /// 是否有权限更新
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<bool> _CanDeleteWidthAuthAsync(string accid, string id)
        {
            return await _Repo.CanDeleteAsync(accid, id);
        }
        #endregion

        #region _CanSave 简单判断数据是否为空或者有权限操作进行更新操作
        /// <summary>
        /// 简单判断数据是否为空或者有权限操作进行更新操作
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<List<string>> _CanSave(string accid, T data)
        {
            var errors = new List<string>();
            //为空验证
            if (data == null)
            {
                errors.Add(ValidityMessage.V_SubmitDataMsg);
                return errors;
            }
            //权限验证
            if (data.IsPersistence())
            {
                var op = await _CanUpdateWidthAuthAsync(accid, data.Id);
                if (!op)
                    errors.Add(ValidityMessage.V_NoPermissionMsg);
            }
            return errors;
        }
        #endregion

        #region _CanDelete 简单判断数据是否为空或者有权限操作进行删除操作
        /// <summary>
        /// 简单判断数据是否为空或者有权限操作进行删除操作
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task<List<string>> _CanDelete(string accid, T data)
        {
            var errors = new List<string>();
            //为空验证
            if (data == null)
            {
                errors.Add(ValidityMessage.V_NotDataOrPermissionMsg);
                return errors;
            }
            //权限验证
            if (data.IsPersistence())
            {
                var op = await _CanDeleteWidthAuthAsync(accid, data.Id);
                if (!op)
                    errors.Add(ValidityMessage.V_NoPermissionMsg);
            }
            return errors;
        }
        #endregion

        /**************** public method ****************/

        #region _GetByIdAsync 根据id信息返回实体数据信息
        /// <summary>
        /// 根据id信息返回实体数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> _GetByIdAsync(string accid, string id)
        {
            return await _Repo.GetAsync(accid, id);
        }
        #endregion

        #region _SaveOrUpdateAsync 更新或者保存实体数据信息
        /// <summary>
        /// 更新或者保存实体数据信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task _SaveOrUpdateAsync(T data)
        {
            if (!data.IsPersistence())
                await _Repo.Context.Set<T>().AddAsync(data);
            await _Repo.SaveChangesAsync();
        }
        #endregion

        #region _DeleteAsync 根据id删除实体数据信息
        /// <summary>
        /// 根据id删除实体数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> _DeleteAsync(string id)
        {
            var data = await _Repo.Context.Set<T>().FindAsync(id);
            if (data != null)
                _Repo.Context.Set<T>().Remove(data);
            return true;
        }
        #endregion
    }
}
