using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class AccountStore : StoreBase<Account>, IStore<Account>
    {
        protected PermissionTreeStore _PermissionTreeStore;

        #region 构造函数
        public AccountStore(ApiDbContext context)
        : base(context)
        {
            _PermissionTreeStore = new PermissionTreeStore(context);
        }
        #endregion

        /**************** protected method ****************/

        #region _OrganPermissionPipe _AccountPermissionPipe
        /// <summary>
        /// 查询用户权限过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected void _AccountPermissionPipe(ref IQueryable<Account> query, Account currentAcc)
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
                        join ps in treeQ on it.Id equals ps.ObjId
                        select it;
            }
            else if (currentAcc.Type == AppConst.AccountType_OrganMember)
            {
                query = query.Take(0);
            }
            else
            {
                query = query.Take(0);
            }
        }
        #endregion

        /**************** public method ****************/

        #region SimplePagedQueryAsync 简单返回分页查询DTO信息
        /// <summary>
        /// 简单返回分页查询DTO信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="desc"></param>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public async Task<PagedData<AccountDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Account, bool>> searchExpression)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Accounts
                            select it;
                //_SearchExpressionPipe(ref query, searchExpression);
                _AccountPermissionPipe(ref query, currentAcc);
                var result = await query.SimplePaging(page, pageSize);
                if (result.Total > 0)
                    return new PagedData<AccountDTO>() { Data = result.Data.Select(x => x.ToDTO()), Total = result.Total, Page = page, Size = pageSize };
            }
            catch (Exception ex)
            {
                Logger.LogError("AccountStore SimplePagedQueryAsync", ex);
            }
            return new PagedData<AccountDTO>();
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AccountDTO> GetByIdAsync(string accid, string id)
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
            return new AccountDTO();
        }
        #endregion

        #region CanCreate 判断用户信息是否符合存储规范
        /// <summary>
        /// 判断用户信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanCreate(string accid, Account data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "用户姓名", 50);
            if (string.IsNullOrWhiteSpace(data.OrganizationId))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "组织");
            if (string.IsNullOrWhiteSpace(data.Type))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "用户类型");
            if (string.IsNullOrWhiteSpace(data.DepartmentId))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "部门");
            if (string.IsNullOrWhiteSpace(data.Mail))
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "邮箱", 50);
            }
            else
            {
                var exist = await _DbContext.Accounts.Where(x => x.Mail == data.Mail.Trim()).CountAsync() > 0;
                if (exist)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "邮箱", data.Mail);
            }

            #region 判断创建人是否拥有相应创建权限
            var currentUser = await _DbContext.Accounts.FindAsync(accid);
            if (currentUser.Type == AppConst.AccountType_SysAdmin || currentUser.Type == AppConst.AccountType_OrganAdmin)
            {
                //sysadmin只能创建organAdmin,sysService
                if (currentUser.Type == AppConst.AccountType_SysAdmin)
                {
                    if (data.Type != AppConst.AccountType_SysService || data.Type != AppConst.AccountType_OrganAdmin)
                        return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
                }
                //organAdmin只能创建orgammember
                if (currentUser.Type == AppConst.AccountType_OrganAdmin)
                {
                    if (data.Type != AppConst.AccountType_OrganMember)
                        return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
                }
            }
            else
            {
                return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
            }
            #endregion

            return string.Empty;
        }
        #endregion

        #region CanUpdate 判断用户信息是否符合更新规范
        /// <summary>
        /// 判断用户信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, Account data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "用户姓名", 50);
            if (string.IsNullOrWhiteSpace(data.Type))
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "用户类型");
            if (string.IsNullOrWhiteSpace(data.Mail))
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "邮箱", 50);
            }
            else
            {
                var exist = await _DbContext.Accounts.Where(x => x.Id != data.Id && x.Mail == data.Mail.Trim()).CountAsync() > 0;
                if (exist)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "邮箱", data.Mail);
            }

            #region 判断更新人是否拥有相应更新权限
            //非本人更新
            if (accid != data.Id)
            {
                var currentUser = await _DbContext.Accounts.FindAsync(accid);
                if (currentUser.Type == AppConst.AccountType_SysAdmin || currentUser.Type == AppConst.AccountType_OrganAdmin)
                {
                    //sysadmin只能更新organAdmin,sysService
                    if (currentUser.Type == AppConst.AccountType_SysAdmin)
                    {
                        if (!(data.Type == AppConst.AccountType_SysService || data.Type == AppConst.AccountType_OrganAdmin))
                            return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
                    }
                    //organAdmin更新创建orgammember
                    if (currentUser.Type == AppConst.AccountType_OrganAdmin)
                    {
                        if (data.Type != AppConst.AccountType_OrganMember)
                            return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
                    }
                }
                else
                {
                    return string.Format(ValidityMessage.V_NoCreateAccPermissionMsg);
                }
            }
            #endregion


            return string.Empty;
        }
        #endregion

        #region CanDelete 判断用户信息是否符合删除规范
        /// <summary>
        /// 判断用户信息是否符合删除规范
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

        #region CanRead 判断用户是否有权限读取信息
        /// <summary>
        /// 判断用户是否有权限读取信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanRead(string accid, string id)
        {
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CreateAsync 新建用户信息
        /// <summary>
        /// 新建用户信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<AccountDTO> CreateAsync(string accid, Account data)
        {
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(data.DepartmentId))
                        data.Department = await _DbContext.Departments.FindAsync(data.DepartmentId);
                    _DbContext.Accounts.Add(data);
                    await _DbContext.SaveChangesAsync();
                    var otree = new PermissionTree();
                    otree.Name = data.Name;
                    otree.OrganizationId = data.OrganizationId;
                    otree.NodeType = AppConst.S_NodeType_Account;
                    otree.ObjId = data.Id;
                    var refDepartmentNode = await _DbContext.PermissionTrees.Where(x => x.ObjId == data.DepartmentId).FirstOrDefaultAsync();
                    if (refDepartmentNode != null)
                    {
                        otree.ParentId = refDepartmentNode.Id;
                        await _PermissionTreeStore.AddChildNode(otree);
                    }
                    tx.Commit();
                    return data.ToDTO();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    Logger.LogError("AccountStore CreateAsync", ex);
                }
            }
            return new AccountDTO();
        }
        #endregion

        #region UpdateAsync 更新用户信息
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<AccountDTO> UpdateAsync(string accid, Account data)
        {
            try
            {
                _DbContext.Accounts.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("AccountStore UpdateAsync", ex);
            }
            return new AccountDTO();
        }
        #endregion
    }
}
