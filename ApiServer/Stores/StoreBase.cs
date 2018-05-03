using ApiModel;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using BambooCommon;

namespace ApiServer.Stores
{
    /// <summary>
    /// StoreBase是所有store仓库类的基类
    /// StoreBase是业务无关的,不关注任何业务信息
    /// StoreBase是DTO无关的,返回的信息只是最原始的实体数据信息
    /// 如果需要返回DTO数据,请在派生Store类里面实现
    /// StoreBase应该是权限无关的,它只做简单操作,如果需要对权限做操作,请在派生类里面实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class StoreBase<T, U>
         where T : class, IEntity, ApiModel.ICloneable, IDTOTransfer<U>, new()
        where U : IData
    {
        protected readonly Repository1<T> _Repo;
        protected readonly ApiDbContext _DbContext;

        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _Repo = new Repository1<T>(context);
            _DbContext = context;
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
        protected async Task<PagedData<T>> _SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<T, bool>> searchPredicate)
        {
            try
            {
                if (page < 1)
                    page = 1;
                if (pageSize < 1)
                    pageSize = SiteConfig.Instance.Json.DefaultPageSize;
                return await _DbContext.Set<T>().Where(x => x.Id != null).Paging1(page, pageSize, orderBy, desc, searchPredicate);
            }
            catch (Exception ex)
            {
                Logger.LogError("_SimplePagedQueryAsync", ex);
            }
            return new PagedData<T>();
        }
        #endregion

        #region _CanSave 简单判断数据是否为空
        /// <summary>
        /// 简单判断数据是否为空
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected List<string> _CanSave(string accid, T data)
        {
            var errors = new List<string>();
            //为空验证
            if (data == null)
            {
                errors.Add(ValidityMessage.V_SubmitDataMsg);
                return errors;
            }
            return errors;
        }
        #endregion

        #region _CanDelete 简单判断数据id是否为空
        /// <summary>
        /// 简单判断数据id是否为空
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected List<string> _CanDelete(string accid, string id)
        {
            var errors = new List<string>();
            //为空验证
            if (id == null)
            {
                errors.Add(ValidityMessage.V_NotDataOrPermissionMsg);
                return errors;
            }
            return errors;
        }
        #endregion

        /**************** public method ****************/

        #region _GetByIdAsync 根据id信息返回实体数据信息
        /// <summary>
        /// 根据id信息返回实体数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> _GetByIdAsync(string id)
        {
            try
            {
                return await _DbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError("_GetByIdAsync", ex);
            }
            return null;
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
            try
            {
                if (!data.IsPersistence())
                    await _DbContext.Set<T>().AddAsync(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("_SaveOrUpdateAsync", ex);
            }
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
            try
            {
                var data = await _DbContext.Set<T>().FindAsync(id);
                if (data != null)
                    _DbContext.Set<T>().Remove(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("_DeleteAsync", ex);
            }
            return true;
        }
        #endregion
    }
}
