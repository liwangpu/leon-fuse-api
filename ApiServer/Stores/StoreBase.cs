using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Services;
using BambooCommon;
using BambooCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
    public class StoreBase<T>
         where T : class, IEntity, ApiModel.ICloneable, new()
    {
        protected readonly ApiDbContext _DbContext;
        protected readonly Repository1<T> _Repo;
        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _DbContext = context;
            //TODO:删除_Repo
            _Repo = new Repository1<T>(context);
        }
        #endregion

        /**************** protected method ****************/

        #region _BasicPermissionPipe 基本的权限树数据过滤管道
        /// <summary>
        /// 基本的权限树数据过滤管道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        /// <returns></returns>
        protected void _BasicPermissionPipe<T>(ref IQueryable<T> query, Account currentAcc)
            where T : IPermission
        {
            if (currentAcc.Type == AppConst.AccountType_SysAdmin)
            {

            }
            else if (currentAcc.Type == AppConst.AccountType_OrganAdmin)
            {
                var treeQ = from ps in _DbContext.PermissionTrees
                            where ps.OrganizationId == currentAcc.OrganizationId && ps.NodeType == AppConst.S_NodeType_Account
                            select ps;
                query = from it in query
                        join ps in treeQ on it.Creator equals ps.ObjId
                        select it;
            }
            else if (currentAcc.Type == AppConst.AccountType_OrganMember)
            {
                query = from it in query
                        where it.Creator == currentAcc.Id
                        select it;
            }
            else
            {
                query = from it in query
                        where it.Creator == currentAcc.Id
                        select it;
            }
        }
        #endregion

        #region 基本查询数据过滤管道 _SearchExpressionPipe
        /// <summary>
        /// 基本查询数据过滤管道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        protected void _SearchExpressionPipe<T>(ref IQueryable<T> query, Expression<Func<T, bool>> searchExpression)
        {
            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }
        }
        #endregion


        protected async Task<PagedData<T>> _SimplePagedQueryAsync1(IQueryable<T> query, int page, int pageSize)
        {
            try
            {
                if (page < 1)
                    page = 1;
                if (pageSize < 1)
                    pageSize = SiteConfig.Instance.Json.DefaultPageSize;
                return await query.SimplePaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError("_SimplePagedQueryAsync", ex);
            }
            return new PagedData<T>();
        }

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
        protected string _CanSave(string accid, T data)
        {
            //为空验证
            if (data == null)
            {
                return ValidityMessage.V_SubmitDataMsg;
            }
            return string.Empty;
        }
        #endregion

        #region _CanDelete 简单判断数据id是否为空
        /// <summary>
        /// 简单判断数据id是否为空
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string _CanDelete(string accid, string id)
        {
            //为空验证
            if (id == null)
            {
                return ValidityMessage.V_NotDataOrPermissionMsg;
            }
            return string.Empty;
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

    //public static class StoreBaseDataPipeExtension
    //{
    //    public static IQueryable<T> TreeLimitedPipe<T>(this IQueryable<T> src)
    //        where T : class, IPermission
    //    {

    //    }
    //}
}
