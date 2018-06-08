using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class AccountStore : StoreBase<Account, AccountDTO>, IStore<Account, AccountDTO>
    {
        protected PermissionTreeStore _PermissionTreeStore;
        #region 构造函数
        public AccountStore(ApiDbContext context)
        : base(context)
        {
            _PermissionTreeStore = new PermissionTreeStore(context);
        }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Account data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(data.Mail))
            {
                var existMail = await _DbContext.Accounts.CountAsync(x => x.Mail == data.Mail) > 0;
                if (existMail)
                    modelState.AddModelError("Mail", "该邮箱已经使用");
            }

            if (!string.IsNullOrWhiteSpace(data.Phone))
            {
                var existPhone = await _DbContext.Accounts.CountAsync(x => x.Phone == data.Phone) > 0;
                if (existPhone)
                    modelState.AddModelError("Phone", "该电话已经使用");
            }

        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, Account data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(data.Mail))
            {
                var existMail = await _DbContext.Accounts.CountAsync(x => x.Mail == data.Mail && x.Id != data.Id) > 0;
                if (existMail)
                    modelState.AddModelError("Mail", "该邮箱已经使用");
            }
            if (!string.IsNullOrWhiteSpace(data.Phone))
            {
                var existPhone = await _DbContext.Accounts.CountAsync(x => x.Phone == data.Phone && x.Id != data.Id) > 0;
                if (existPhone)
                    modelState.AddModelError("Phone", "该电话已经使用");
            }
        }
        #endregion

        #region CanCreateAsync 判断数据是否满足创建规范
        /// <summary>
        /// CanCreateAsync
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public override async Task<bool> CanCreateAsync(string accid)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            if (currentAcc == null)
                return false;

            if (currentAcc.Type == AppConst.AccountType_SysAdmin)
            {
                return true;
            }
            else if (currentAcc.Type == AppConst.AccountType_BrandAdmin)
            {
                return true;
            }
            else
            {

            }
            return await Task.FromResult(false);
        }
        #endregion

        #region CreateAsync 新建用户信息
        /// <summary>
        /// 新建用户信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override async Task CreateAsync(string accid, Account data)
        {
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(data.DepartmentId))
                        data.Department = await _DbContext.Departments.FindAsync(data.DepartmentId);
                    await base.CreateAsync(accid, data);
                    var otree = new PermissionTree();
                    otree.Name = data.Name;
                    //为了确保OrganizationId和Organization只赋一个导致OrganizationId被遗漏
                    if (!string.IsNullOrWhiteSpace(data.OrganizationId))
                        otree.OrganizationId = data.OrganizationId;
                    if (data.Organization != null)
                        otree.OrganizationId = data.Organization.Id;
                    otree.NodeType = AppConst.S_NodeType_Account;
                    otree.ObjId = data.Id;
                    var refDepartmentNode = await _DbContext.PermissionTrees.Where(x => x.ObjId == data.DepartmentId).FirstOrDefaultAsync();
                    if (refDepartmentNode != null)
                    {
                        otree.ParentId = refDepartmentNode.Id;
                        await _PermissionTreeStore.AddChildNode(otree);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        #region _GetPermissionData 获取操作权限数据
        /// <summary>
        /// 获取操作权限数据
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="dataOp"></param>
        /// <param name="withInActive"></param>
        /// <returns></returns>
        public override async Task<IQueryable<Account>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            var emptyQuery = Enumerable.Empty<Account>().AsQueryable();
            var query = emptyQuery;

            var currentAcc = await _DbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accid);
            if (currentAcc == null)
                return query;

            //数据状态
            if (withInActive)
                query = _DbContext.Set<Account>();
            else
                query = _DbContext.Set<Account>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);


            if (currentAcc.Type == AppConst.AccountType_SysAdmin)
            {
                return query;
            }
            else
            {
                //组织管理员(包括合伙人|供应商)能管理自己组织下普通用户和第一层级管理员信息
                //组织所属管理员可以管理自己组织管理员

                #region [U,D]
                if (dataOp == DataOperateEnum.Update || dataOp == DataOperateEnum.Delete)
                {
                    var ownOrganUserQ = query.Where(x => x.OrganizationId == currentAcc.OrganizationId);
                    var onLevelDownOrganIdsQ = _DbContext.Organizations.Where(x => x.ParentId == currentAcc.OrganizationId).Select(x => x.Id);
                    var oneLevelDownOrganAdminQ = query.Where(x => onLevelDownOrganIdsQ.Contains(x.OrganizationId));
                    return ownOrganUserQ.Union(oneLevelDownOrganAdminQ);
                }
                #endregion

                #region [R]
                if (dataOp == DataOperateEnum.Read)
                {
                    var ownOrganIdsQ = (await _PermissionTreeStore.GetOrganManageNode(currentAcc.OrganizationId, new List<string>() { AppConst.S_NodeType_Organization }, true)).Select(x => x.ObjId);
                    return query.Where(x => ownOrganIdsQ.Contains(x.OrganizationId));
                }
                #endregion

            }
            return emptyQuery;
        }
        #endregion
    }
}
