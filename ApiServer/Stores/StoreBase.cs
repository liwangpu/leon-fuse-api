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

        /// <summary>
        /// 资源访问类型
        /// </summary>
        public virtual ResourceTypeEnum ResourceTypeSetting
        {
            get
            {
                return ResourceTypeEnum.Personal;
            }
        }

        #region 构造函数
        public StoreBase(ApiDbContext context)
        {
            _DbContext = context;
        }
        #endregion

        /**************** protected method ****************/

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

        #region virtual _GetPermissionData 获取用户权限数据
        /// <summary>
        /// 获取用户权限数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="dataOp"></param>
        /// <param name="withInActive"></param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            var emptyQuery = Enumerable.Empty<T>().AsQueryable();
            var query = emptyQuery;

            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            if (currentAcc == null)
                return query;

            //数据状态
            if (withInActive)
                query = _DbContext.Set<T>();
            else
                query = _DbContext.Set<T>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            #region 用户角色类型过滤(为高级用户角色定义查询,这些高级用户角色不走资源类型和数据操作类型过滤)
            {
                if (currentAcc.Type == AppConst.AccountType_SysAdmin)
                {
                    return query;
                }
                else if (currentAcc.Type == AppConst.AccountType_OrganAdmin)
                {
                    return query.Where(x => x.OrganizationId == currentAcc.OrganizationId);
                }
                else
                {
                    //剩下走 资源类型过滤-数据操作类型过滤
                }
            }
            #endregion

            #region 资源类型过滤-数据操作类型过滤(基本用户类型,不用考虑角色了,只考虑资源类型和操作过滤)
            {
                #region getPersonalResource 不单独定义方法了,用一个Func返回query就好了
                var getPersonalResource = new Func<IQueryable<T>, IQueryable<T>>((q) =>
                {
                    return q.Where(x => x.Creator == currentAcc.Id);
                });
                #endregion

                if (ResourceTypeSetting == ResourceTypeEnum.NoLimit)
                {
                    return query;
                }
                else if (ResourceTypeSetting == ResourceTypeEnum.Organizational)
                {
                    if (dataOp == DataOperateEnum.Read)
                        return query.Where(x => x.OrganizationId == currentAcc.OrganizationId);
                    else
                        return getPersonalResource(query);
                }
                else
                {
                    return getPersonalResource(query);
                }
            }
            #endregion
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

        #region virtual ExistAsync 判断id对应记录是否存在
        /// <summary>
        /// 判断id对应记录是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="withInActive"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistAsync(string id, bool withInActive = false)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (withInActive)
                    return await _DbContext.Set<T>().CountAsync(x => x.Id == id) > 0;
                return await _DbContext.Set<T>().CountAsync(x => x.Id == id && x.ActiveFlag == AppConst.I_DataState_Active) > 0;
            }
            return false;
        }
        #endregion

        #region virtual CanCreateAsync 判断用户是否有权限创建数据
        /// <summary>
        /// 判断用户是否有权限创建数据
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanCreateAsync(string accid)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            if (currentAcc == null)
                return false;

            if (currentAcc.Type == AppConst.AccountType_SysAdmin)
            {
                return true;
            }
            else if (currentAcc.Type == AppConst.AccountType_OrganAdmin)
            {
                if (ResourceTypeSetting == ResourceTypeEnum.Organizational)
                    return true;
            }
            else
            {
                if (ResourceTypeSetting == ResourceTypeEnum.Personal)
                    return true;
            }
            return await Task.FromResult(false);
        }
        #endregion

        #region virtual CanUpdateAsync 判断用户是否有权限更新数据
        /// <summary>
        /// 判断用户是否有权限更新数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanUpdateAsync(string accid, string id)
        {
            var query = await _GetPermissionData(accid, DataOperateEnum.Update, true);
            return await query.Where(x => x.Id == id).CountAsync() > 0;
        }
        #endregion

        #region virtual CanDeleteAsync 判断用户是否有权限删除数据
        /// <summary>
        /// 判断用户是否有权限删除数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanDeleteAsync(string accid, string id)
        {
            var query = await _GetPermissionData(accid, DataOperateEnum.Delete, true);
            return await query.Where(x => x.Id == id).CountAsync() > 0;
        }
        #endregion

        #region virtual CanReadAsync 判断用户是否有权限读取数据
        /// <summary>
        /// 判断用户是否有权限读取数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CanReadAsync(string accid, string id)
        {
            var query = await _GetPermissionData(accid, DataOperateEnum.Read, true);
            return await query.Where(x => x.Id == id).CountAsync() > 0;
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
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            data.Id = GuidGen.NewGUID();
            //如果创建人和修改人有指定,说明有自定义需要,应该按传入的参数处理
            if (string.IsNullOrWhiteSpace(data.Creator))
                data.Creator = accid;
            if (string.IsNullOrWhiteSpace(data.Modifier))
                data.Modifier = accid;
            if (string.IsNullOrWhiteSpace(data.OrganizationId))
                data.OrganizationId = currentAcc.OrganizationId;
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
        /// <param name="advanceQuery">注意,一旦有高级过滤条件,默认非活动数据也显示,请自行对ActiveFlag做过滤</param>
        /// <returns></returns>
        public virtual async Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null)
        {
            //读取设置也取非活动状态数据,后面在advanceQuery再修改是否真的需要
            var query = await _GetPermissionData(accid, DataOperateEnum.Read, true);
            if (advanceQuery != null)
                query = await advanceQuery(query);
            else
                query = query.Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            _QSearchFilter(ref query, model.Q);
            _KeyWordSearchFilter(ref query, model.Search);
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
