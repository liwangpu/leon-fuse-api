using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using BambooCommon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /**************** public method ****************/

        #region CanCreate 判断用户信息是否符合存储规范
        /// <summary>
        /// 判断用户信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanCreate(string accid, Account data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "用户名称", 50));

            #region 判断创建人是否拥有相应创建权限
            var currentUser = await _DbContext.Accounts.FindAsync(accid);
            if (currentUser.Type == AppConst.AccountType_SysAdmin || currentUser.Type == AppConst.AccountType_OrganAdmin)
            {
                //sysadmin只能创建organAdmin,sysService
                if (currentUser.Type == AppConst.AccountType_SysAdmin)
                {
                    if (data.Type != AppConst.AccountType_SysService || data.Type != AppConst.AccountType_OrganAdmin)
                        errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
                }
                //organAdmin只能创建orgammember
                if (currentUser.Type == AppConst.AccountType_OrganAdmin)
                {
                    if (data.Type != AppConst.AccountType_OrganMember)
                        errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
                }
            }
            else
            {
                errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
            }
            #endregion

            return errors;
        }
        #endregion

        #region CanUpdate 判断用户信息是否符合更新规范
        /// <summary>
        /// 判断用户信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanUpdate(string accid, Account data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "用户名称", 50));

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
                        if (data.Type != AppConst.AccountType_SysService || data.Type != AppConst.AccountType_OrganAdmin)
                            errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
                    }
                    //organAdmin更新创建orgammember
                    if (currentUser.Type == AppConst.AccountType_OrganAdmin)
                    {
                        if (data.Type != AppConst.AccountType_OrganMember)
                            errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
                    }
                }
                else
                {
                    errors.Add(string.Format(ValidityMessage.V_NoCreateAccPermissionMsg));
                }
            }
            #endregion


            return errors;
        }
        #endregion

        #region CanDelete 判断用户信息是否符合删除规范
        /// <summary>
        /// 判断用户信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanDelete(string accid, string id)
        {
            var errors = new List<string>();
            var valid = _CanDelete(accid, id);
            if (valid.Count > 0)
                return valid;
            return errors;
        }
        #endregion

        #region CanRead 判断用户是否有权限读取信息
        /// <summary>
        /// 判断用户是否有权限读取信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CanRead(string accid, string id)
        {
            var errors = new List<string>();
            return errors;
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
