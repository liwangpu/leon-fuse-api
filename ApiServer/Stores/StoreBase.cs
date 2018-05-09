using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    /// <summary>
    /// StoreBase是所有store仓库类的基类
    /// StoreBase是业务无关的,不关注任何业务信息
    /// StoreBase是DTO无关的,返回的信息只是最原始的实体数据信息
    /// 如果需要返回DTO数据,请在派生Store类里面实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoreBase<T>
         where T : class, IEntity, IDTOTransfer<IData>, new()
    {
        protected readonly ApiDbContext _DbContext;

        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _DbContext = context;
        }
        #endregion

        /**************** protected method ****************/

        #region _BasicPipe 基本的数据过滤管道
        /// <summary>
        /// 基本的数据过滤管道
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected void _BasicPipe(ref IQueryable<T> query, Account currentAcc)
        {
            if (currentAcc != null)
            {
                if (currentAcc.Type == AppConst.AccountType_SysAdmin)
                {
                    //TODO:管理员或者特殊角色可能需要查看InActive数据,待商榷
                    query = from it in query
                            where it.ActiveFlag == AppConst.I_DataState_Active
                            select it;
                }
                else
                {
                    query = from it in query
                            where it.ActiveFlag == AppConst.I_DataState_Active
                            select it;
                }
            }
            else
            {
                query = query.Take(0);
            }
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
            if (currentAcc != null)
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
            else
            {
                query = query.Take(0);
            }
        }
        #endregion

        #region _KeyWordSearchPipe 基本关键字过滤管道 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        protected void _KeyWordSearchPipe(ref IQueryable<T> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }
        #endregion

        #region _OrderByPipe 基本排序过滤管道
        /// <summary> 
        /// 基本排序过滤管道
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        protected void _OrderByPipe(ref IQueryable<T> query, string orderBy, bool desc)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                /*
                 * api提供的字段信息是大小写不敏感,或者说经过转换大小写了
                 * client在排序的时候并不知道真实属性的名字,需要经过反射获取原来属性的名字信息
                 * 确保在排序的时候不会出现异常
                 */
                var realProperty = string.Empty;
                var properties = typeof(T).GetProperties();
                for (int idx = properties.Length - 1; idx >= 0; idx--)
                {
                    var propName = properties[idx].Name.ToString();
                    if (propName.ToLower() == orderBy.ToLower())
                    {
                        realProperty = propName;
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(realProperty))
                {
                    if (desc)
                        query = query.OrderByDescendingBy(realProperty);
                    else
                        query = query.OrderBy(realProperty);
                }
            }
        }
        #endregion

        /**************** public method ****************/

        #region virtual GetById 根据id信息返回实体数据信息
        /// <summary>
        /// 根据id信息返回实体数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(string id)
        {
            try
            {
                return _DbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                Logger.LogError("GetById", ex);
            }
            return null;
        }
        #endregion

        #region virtual GetByIdAsync 根据id信息返回实体数据信息
        /// <summary>
        /// 根据id信息返回实体数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(string id)
        {
            try
            {
                return await _DbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError("GetByIdAsync", ex);
            }
            return null;
        }
        #endregion

        #region virtual Exist 简单判断id对应记录是否存在(InActive状态类似不存在,返回false)
        /// <summary>
        /// 简单判断id对应记录是否存在(InActive状态类似不存在,返回false)
        /// 提供虚方法以便复杂业务逻辑判断存在重写
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> Exist(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
                return await _DbContext.Set<T>().CountAsync(x => x.Id == id && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
            return false;
        }
        #endregion

        #region virtual CanCreate 判断用户是否有权限创建数据
        /// <summary>
        /// CanCreate
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanCreate(string accid)
        {
            return await Task.FromResult(true);
        }
        #endregion

        #region virtual CanUpdate 判断用户是否有权限更新数据
        /// <summary>
        /// 判断用户是否有权限更新数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanUpdate(string accid, string id)
        {
            return await CanRead(accid, id);
        }
        #endregion

        #region virtual CanDelete 判断用户是否有权限删除数据
        /// <summary>
        /// 判断用户是否有权限删除数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanDelete(string accid, string id)
        {
            return await CanRead(accid, id);
        }
        #endregion

        #region virtual CanRead 判断用户是否有权限读取数据
        /// <summary>
        /// 判断用户是否有权限读取数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanRead(string accid, string id)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Set<T>()
                        select it;
            _BasicPipe(ref query, currentAcc);
            _BasicPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return false;
            return true;
        }
        #endregion

        #region virtual CreateAsync 新建实体信息
        /// <summary>
        /// 新建实体信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(string accid, T data)
        {
            try
            {
                data.Id = GuidGen.NewGUID();
                data.Creator = accid;
                data.Modifier = accid;
                data.CreatedTime = DateTime.Now;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Set<T>().Add(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("CreateAsync", ex);
            }
        }
        #endregion

        #region virtual UpdateAsync 更新实体信息
        /// <summary>
        /// 更新实体信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(string accid, T data)
        {
            try
            {
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                _DbContext.Set<T>().Update(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("UpdateAsync", ex);
            }
        }
        #endregion

        #region virtual DeleteAsync 删除实体信息
        /// <summary>
        /// 删除材质信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string accid, string id)
        {
            try
            {
                var data = await GetByIdAsync(id);
                data.Modifier = accid;
                data.ModifiedTime = DateTime.Now;
                data.ActiveFlag = AppConst.I_DataState_InActive;
                _DbContext.Set<T>().Update(data);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("DeleteAsync", ex);
            }
        }
        #endregion

        #region virtual SimplePagedQueryAsync 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual async Task<PagedData<T>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, string search)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Set<T>()
                            select it;
                _BasicPipe(ref query, currentAcc);
                _OrderByPipe(ref query, orderBy, desc);
                _KeyWordSearchPipe(ref query, search);
                _BasicPermissionPipe(ref query, currentAcc);
                return await query.SimplePaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError("SimplePagedQueryAsync", ex);
            }
            return new PagedData<T>();
        }
        #endregion

        /**************** public static method ****************/

        #region static PageQueryDTOTransfer 将分页查询PagedData转为PagedData DTO
        /// <summary>
        /// 将分页查询PagedData转为PagedData DTO
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static PagedData<IData> PageQueryDTOTransfer(PagedData<T> result)
        {
            if (result != null)
            {
                if (result.Total > 0)
                {
                    return new PagedData<IData>() { Data = result.Data.Select(x => x.ToDTO()).ToList(), Page = result.Page, Size = result.Size, Total = result.Total };
                }
            }
            return new PagedData<IData>();
        }
        #endregion

    }
}
