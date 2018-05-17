using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
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
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="DTO"></typeparam>
    public class StoreBase<T, DTO>
         where T : class, IEntity, IDTOTransfer<DTO>, new()
        where DTO : class, IData, new()
    {
        protected readonly ApiDbContext _DbContext;

        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _DbContext = context;
        }
        #endregion

        /**************** protected method ****************/

        #region virtual _PermissionTreeResourceFilter 权限树资源类型过滤
        /// <summary>
        /// 权限树资源类型过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected virtual void _PermissionTreeResourceFilter(ref IQueryable<T> query, Account currentAcc)
        {
            //TODO:待完善权限树获取资源类型

            //var treeQ = from ps in _DbContext.PermissionTrees
            //            where ps.OrganizationId == currentAcc.OrganizationId && ps.NodeType == AppConst.S_NodeType_Account
            //            select ps;
            //query = from it in query
            //        join ps in treeQ on it.Creator equals ps.ObjId
            //        select it;
        }
        #endregion

        #region virtual _PersonalResourceFilter 获取个人资源
        /// <summary>
        /// 获取个人资源
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected virtual void _PersonalResourceFilter(ref IQueryable<T> query, Account currentAcc)
        {
            query = from it in query
                    where it.Creator == currentAcc.Id
                    select it;
        }
        #endregion

        #region virtual _DepartmentalResourceFilter 获取部门共享资源
        /// <summary>
        /// 获取部门共享资源
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected virtual void _DepartmentalResourceFilter(ref IQueryable<T> query, Account currentAcc)
        {

        }
        #endregion

        #region virtual _OrganizationalResourceFilter 获取组织共享资源
        /// <summary>
        /// 获取组织共享资源
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected virtual void _OrganizationalResourceFilter(ref IQueryable<T> query, Account currentAcc)
        {
            var treeQ = from ps in _DbContext.PermissionTrees
                        where ps.OrganizationId == currentAcc.OrganizationId && ps.NodeType == AppConst.S_NodeType_Account
                        select ps;
            query = from it in query
                    join ps in treeQ on it.Creator equals ps.ObjId
                    select it;
        }
        #endregion

        #region _QSearchFilter 查询参数过滤
        /// <summary>
        /// 查询参数过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="q"></param>
        protected void _QSearchFilter(ref IQueryable<T> query, string q)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                var wheres = QueryParser.Parse<T>(q);
                foreach (var item in wheres)
                    query = query.Where(item);
            }
        }
        #endregion

        #region _BasicFilter 基本的数据过滤
        /// <summary>
        /// 基本的数据过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected void _BasicFilter(ref IQueryable<T> query, Account currentAcc)
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

        #region tmp
        //protected IQueryable<T> _BasicResourceTypeFilter(IQueryable<T> query, Account account, PermissionTree organNode, PermissionTree departmentNode, DataOperateEnum operate)
        //{
        //    #region 获取全开放状态的资源
        //    {
        //        var dataQ = from rs in query
        //                    where rs.ResourceType >= (int)ResourceTypeEnum.NoLimit + (int)operate * AppConst.I_Permission_GradeStep
        //                    select rs;
        //        query = dataQ;

        //        var sql = query.ToString();
        //    }
        //    #endregion

        //    #region 组织开放状态资源
        //    {
        //        //var dataQ = from rs in query
        //        //            where rs.ResourceType >= (int)ResourceTypeEnum.Organizational + (int)operate * AppConst.I_Permission_GradeStep
        //        //            select rs;
        //        //var treeQ = from ps in _DbContext.PermissionTrees
        //        //            where ps.OrganizationId == organNode.ObjId && ps.NodeType == AppConst.S_NodeType_Account
        //        //            select ps;
        //        //var typeQ = from rs in dataQ
        //        //            join ps in treeQ on rs.Creator equals ps.ObjId
        //        //            select rs;
        //        //query = query.Union(typeQ);
        //    }
        //    #endregion

        //    #region 部门开放状态资源
        //    {
        //        //var dataQ = from rs in query
        //        //            where rs.ResourceType >= (int)ResourceTypeEnum.Organizational + (int)operate * AppConst.I_Permission_GradeStep
        //        //            select rs;
        //        //var treeQ = from ps in _DbContext.PermissionTrees
        //        //            where ps.OrganizationId == organNode.ObjId && ps.LValue > departmentNode.LValue && ps.RValue < departmentNode.RValue && ps.NodeType == AppConst.S_NodeType_Account
        //        //            select ps;
        //        //var typeQ = from rs in dataQ
        //        //            join ps in treeQ on rs.Creator equals ps.ObjId
        //        //            select rs;
        //        //query = query.Union(typeQ);
        //    }
        //    #endregion

        //    #region 个人资源
        //    {
        //        //var dataQ = from it in query
        //        //            where it.Creator == account.Id
        //        //            select it;
        //        //query = query.Union(dataQ);
        //    }
        //    #endregion

        //    return query;
        //} 
        #endregion

        #region _BasicPermissionFilter 基本的权限树数据过滤
        /// <summary>
        /// 基本的权限树数据过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        /// <param name="accountNode"></param>
        /// <param name="organNode"></param>
        /// <param name="departmentNode"></param>
        /// <param name="resType"></param>
        protected void _BasicPermissionFilter(ref IQueryable<T> query, Account currentAcc, PermissionTree accountNode, PermissionTree organNode, PermissionTree departmentNode, ResourceTypeEnum resType)
        {
            if (currentAcc != null)
            {
                if (currentAcc.Type == AppConst.AccountType_SysAdmin)
                {

                }
                else if (currentAcc.Type == AppConst.AccountType_OrganAdmin)
                {
                    _OrganizationalResourceFilter(ref query, currentAcc);
                }
                else if (currentAcc.Type == AppConst.AccountType_OrganMember)
                {
                    if (resType == ResourceTypeEnum.Organizational)
                        _OrganizationalResourceFilter(ref query, currentAcc);
                    else if (resType == ResourceTypeEnum.Departmental)
                        _DepartmentalResourceFilter(ref query, currentAcc);
                    else
                        _PersonalResourceFilter(ref query, currentAcc);
                }
                else
                {
                    _PersonalResourceFilter(ref query, currentAcc);
                }
            }
            else
            {
                query = query.Take(0);
            }
        }
        #endregion

        #region _KeyWordSearchFilter 基本关键字过滤
        /// <summary>
        /// 基本关键字过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        protected void _KeyWordSearchFilter(ref IQueryable<T> query, string search)
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
            //默认以修改时间作为排序
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "ModifiedTime";
                desc = true;
            }

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

        #region _GetByIdAsync 根据id信息返回实体数据信息
        /// <summary>
        /// 根据id信息返回实体数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> _GetByIdAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var data = await _DbContext.Set<T>().FindAsync(id);
                if (data != null)
                    return data;
            }
            return new T();
        }
        #endregion

        #region virtual GetByIdAsync 根据id信息返回实体DTO数据信息
        /// <summary>
        /// 根据id信息返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DTO> GetByIdAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var data = await _DbContext.Set<T>().FindAsync(id);
                if (data != null)
                    return data.ToDTO();
            }
            return new DTO();
        }
        #endregion

        #region virtual ExistAsync 简单判断id对应记录是否存在(InActive状态类似不存在,返回false)
        /// <summary>
        /// 简单判断id对应记录是否存在(InActive状态类似不存在,返回false)
        /// 提供虚方法以便复杂业务逻辑判断存在重写
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
                return await _DbContext.Set<T>().CountAsync(x => x.Id == id && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
            return false;
        }
        #endregion

        #region virtual CanCreateAsync 判断用户是否有权限创建数据
        /// <summary>
        /// 判断用户是否有权限创建数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanCreateAsync(string accid, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            return await Task.FromResult(true);
        }
        #endregion

        #region virtual CanUpdateAsync 判断用户是否有权限更新数据
        /// <summary>
        /// 判断用户是否有权限更新数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanUpdateAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            return await CanReadAsync(accid, id, resType);
        }
        #endregion

        #region virtual CanDeleteAsync 判断用户是否有权限删除数据
        /// <summary>
        /// 判断用户是否有权限删除数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanDeleteAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            return await CanReadAsync(accid, id, resType);
        }
        #endregion

        #region virtual CanReadAsync 判断用户是否有权限读取数据
        /// <summary>
        /// 判断用户是否有权限读取数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanReadAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var accountNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.Id);
            var organNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.OrganizationId);
            var departmentNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.DepartmentId);
            var query = from it in _DbContext.Set<T>()
                        select it;
            _BasicFilter(ref query, currentAcc);
            _BasicPermissionFilter(ref query, currentAcc, accountNode, organNode, departmentNode, resType);
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
            data.Id = GuidGen.NewGUID();
            //如果创建人和修改人有指定,说明有自定义需要,应该按传入的参数处理
            if (string.IsNullOrWhiteSpace(data.Creator))
                data.Creator = accid;
            if (string.IsNullOrWhiteSpace(data.Modifier))
                data.Modifier = accid;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = DateTime.Now;
            _DbContext.Set<T>().Add(data);
            await _DbContext.SaveChangesAsync();
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
            //如果修改人有指定,说明有自定义需要,应该按传入的参数处理
            if (string.IsNullOrWhiteSpace(data.Modifier))
                data.Modifier = accid;
            data.ModifiedTime = DateTime.Now;
            _DbContext.Set<T>().Update(data);
            await _DbContext.SaveChangesAsync();
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
            var data = await _GetByIdAsync(id);
            data.Modifier = accid;
            data.ModifiedTime = DateTime.Now;
            data.ActiveFlag = AppConst.I_DataState_InActive;
            _DbContext.Set<T>().Update(data);
            await _DbContext.SaveChangesAsync();
        }
        #endregion

        #region virtual SimplePagedQueryAsync 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accid"></param>
        /// <param name="resType"></param>
        /// <returns></returns>
        public virtual async Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid, ResourceTypeEnum resType = ResourceTypeEnum.Personal)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var accountNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.Id);
            var organNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.OrganizationId);
            var departmentNode = await _DbContext.PermissionTrees.FirstOrDefaultAsync(x => x.ObjId == currentAcc.DepartmentId);
            var query = from it in _DbContext.Set<T>()
                        select it;
            _QSearchFilter(ref query, model.Q);
            _BasicFilter(ref query, currentAcc);
            _KeyWordSearchFilter(ref query, model.Search);
            _BasicPermissionFilter(ref query, currentAcc, accountNode, organNode, departmentNode, resType);
            _OrderByPipe(ref query, model.OrderBy, model.Desc);
            return await query.SimplePaging(model.Page, model.PageSize);
        }
        #endregion

        /**************** public static method ****************/

        #region static PageQueryDTOTransfer 将分页查询PagedData转为PagedData DTO
        /// <summary>
        /// 将分页查询PagedData转为PagedData DTO
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static PagedData<DTO> PageQueryDTOTransfer(PagedData<T> result)
        {
            if (result != null)
            {
                if (result.Total > 0)
                {
                    return new PagedData<DTO>() { Data = result.Data.Select(x => x.ToDTO()).ToList(), Page = result.Page, Size = result.Size, Total = result.Total };
                }
            }
            return new PagedData<DTO>();
        }
        #endregion


    }
}
