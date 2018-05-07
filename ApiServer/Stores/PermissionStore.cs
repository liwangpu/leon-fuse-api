using ApiModel;
using ApiModel.Consts;
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
    public class PermissionStore<T> : StoreBase<T>
               where T : class, IEntity, IPermission, new()
    {
        #region 构造函数
        public PermissionStore(ApiDbContext context)
        : base(context)
        {

        }
        #endregion

        #region _BasicPermissionPipe 基本的权限树数据过滤管道
        /// <summary>
        /// 基本的权限树数据过滤管道
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected void _BasicPermissionPipe(ref IQueryable<T> query, Account currentAcc)
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

        #region _SimplePagedQueryWithPermissionAsync 权限相关的分页查询
        /// <summary>
        /// 权限相关的分页查询
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        protected async Task<PagedData<T>> _SimplePagedQueryWithPermissionAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<T, bool>> searchExpression)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Set<T>()
                            select it;
                _OrderByPipe(ref query, orderBy, desc);
                _SearchExpressionPipe(ref query, searchExpression);
                _BasicPermissionPipe(ref query, currentAcc);
                return await query.SimplePaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError("_SimplePagedQueryAsync", ex);
            }
            return new PagedData<T>();
        }
        #endregion

    }
}
