using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Services;
using BambooCommon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class OrganizationStore : StoreBase<Organization>, IStore<Organization>
    {
        protected PermissionTreeStore _PermissionTreeStore;
        protected DepartmentStore _DepartmentStore;
        protected AccountStore _AccountStore;

        #region 构造函数
        public OrganizationStore(ApiDbContext context)
            : base(context)
        {
            _PermissionTreeStore = new PermissionTreeStore(context);
            _DepartmentStore = new DepartmentStore(context);
            _AccountStore = new AccountStore(context);
        }
        #endregion


        /**************** public method ****************/

        #region CanCreate 判断组织信息是否符合存储规范
        /// <summary>
        /// 判断组织信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanCreate(string accid, Organization data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "组织名称", 50));
            if (string.IsNullOrWhiteSpace(data.Mail) || data.Mail.Length > 50)
            {
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "邮箱", 50));
            }
            else
            {
                var exist = await _DbContext.Accounts.Where(x => x.Mail == data.Mail.Trim()).CountAsync();
                if (exist > 0)
                    errors.Add(string.Format(ValidityMessage.V_DuplicatedMsg, "邮箱", data.Mail.Trim()));
            }


            return errors;
        }
        #endregion

        #region CanUpdate 判断组织信息是否符合更新规范
        /// <summary>
        /// 判断组织信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanUpdate(string accid, Organization data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "产品名称", 50));
            return errors;
        }
        #endregion

        #region CanDelete 判断组织信息是否符合删除规范
        /// <summary>
        /// 判断组织信息是否符合删除规范
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

        #region CreateAsync 新建组织信息
        /// <summary>
        /// 新建组织信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<OrganizationDTO> CreateAsync(string accid, Organization data)
        {
            var bCreateDefaultResource = false;
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    _DbContext.Organizations.Add(data);
                    await _DbContext.SaveChangesAsync();
                    var otree = new PermissionTree();
                    otree.Name = data.Name;
                    otree.OrganizationId = data.Id;
                    otree.NodeType = AppConst.S_NodeType_Organization;
                    otree.ObjId = data.Id;
                    if (string.IsNullOrWhiteSpace(data.ParentId))
                    {
                        await _PermissionTreeStore.AddNewNode(otree);
                        bCreateDefaultResource = true;
                    }
                    else
                    {
                        var parentOrganNode = _DbContext.PermissionTrees.Where(x => x.ObjId == data.ParentId).FirstOrDefault();
                        if (parentOrganNode != null)
                        {
                            otree.ParentId = parentOrganNode.Id;
                            otree.OrganizationId = data.ParentId;
                            await _PermissionTreeStore.AddChildNode(otree);
                        }
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    bCreateDefaultResource = false;
                    Logger.LogError("OrganizationStore CreateAsync", ex);
                    return new OrganizationDTO();
                }
            }


            if (bCreateDefaultResource)
            {
                #region 创建默认部门
                var department = new Department();
                department.Organization = data;
                department.Name = data.Name;
                department.Creator = accid;
                department.Modifier = accid;
                await _DepartmentStore.CreateAsync(accid, department);
                #endregion

                #region 创建默认管理员
                var account = new Account();
                account.Type = AppConst.AccountType_OrganAdmin;
                account.Name = "组织管理员";
                account.Mail = data.Mail;
                account.Password = ConstVar.DefaultNormalPasswordMd5;
                account.Location = data.Location;
                account.Creator = accid;
                account.Modifier = accid;
                account.ExpireTime = DateTime.Now.AddYears(10);
                account.ActivationTime = DateTime.UtcNow;
                account.Department = department;
                account.DepartmentId = department.Id;
                account.Organization = data;
                account.OrganizationId = data.Id;
                await _AccountStore.CreateAsync(accid, account);
                #endregion
            }
            return data.ToDTO();
        }
        #endregion

        #region UpdateAsync 更新组织信息
        /// <summary>
        /// 更新组织信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<OrganizationDTO> UpdateAsync(string accid, Organization data)
        {
            try
            {
                _DbContext.Organizations.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("OrganizationStore UpdateAsync", ex);
            }
            return new OrganizationDTO();
        }
        #endregion
    }
}
