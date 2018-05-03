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
    public class DepartmentStore : StoreBase<Department>, IStore<Department>
    {
        protected PermissionTreeStore _PermissionTreeStore;

        #region 构造函数
        public DepartmentStore(ApiDbContext context)
        : base(context)
        {
            _PermissionTreeStore = new PermissionTreeStore(context);
        }
        #endregion

        /**************** public method ****************/

        #region CanCreate 判断部门信息是否符合存储规范
        /// <summary>
        /// 判断部门信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanCreate(string accid, Department data)
        {
            var errors = new List<string>();
            var valid = _CanSave(accid, data);
            if (valid.Count > 0)
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_StringLengthRejectMsg, "部门名称", 50));
            if (string.IsNullOrWhiteSpace(data.OrganizationId) || data.OrganizationId.Length > 50)
                errors.Add(string.Format(ValidityMessage.V_RequiredRejectMsg, "组织编号"));

            return errors;
        }
        #endregion

        #region CanUpdate 判断部门信息是否符合更新规范
        /// <summary>
        /// 判断部门信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<string>> CanUpdate(string accid, Department data)
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

        #region CanDelete 判断部门信息是否符合删除规范
        /// <summary>
        /// 判断部门信息是否符合删除规范
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
        public async Task<DepartmentDTO> CreateAsync(string accid, Department data)
        {
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    _DbContext.Departments.Add(data);
                    await _DbContext.SaveChangesAsync();
                    var otree = new PermissionTree();
                    otree.Name = data.Name;
                    otree.OrganizationId = data.OrganizationId;
                    otree.NodeType = AppConst.S_NodeType_Department;
                    otree.ObjId = data.Id;
                    if (string.IsNullOrWhiteSpace(data.ParentId))
                    {
                        var refOrganNode = await _DbContext.PermissionTrees.Where(x => x.ObjId == data.OrganizationId).FirstOrDefaultAsync();
                        if (refOrganNode != null)
                        {
                            otree.ParentId = refOrganNode.Id;
                            await _PermissionTreeStore.AddChildNode(otree);
                        }
                    }
                    else
                    {
                        var parentDepartmentNode = await _DbContext.PermissionTrees.Where(x => x.ObjId == data.ParentId).FirstOrDefaultAsync();
                        if (parentDepartmentNode != null)
                        {
                            otree.ParentId = parentDepartmentNode.Id;
                            otree.OrganizationId = data.OrganizationId;
                            await _PermissionTreeStore.AddChildNode(otree);
                        }
                    }
                    tx.Commit();
                    return data.ToDTO();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    Logger.LogError("DepartmentStore CreateAsync", ex);
                }
            }
            return new DepartmentDTO();
        }
        #endregion

        #region UpdateAsync 更新组织信息
        /// <summary>
        /// 更新组织信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<DepartmentDTO> UpdateAsync(string accid, Department data)
        {
            try
            {
                _DbContext.Departments.Update(data);
                await _DbContext.SaveChangesAsync();
                return data.ToDTO();
            }
            catch (Exception ex)
            {
                Logger.LogError("DepartmentStore UpdateAsync", ex);
            }
            return new DepartmentDTO();
        }
        #endregion
    }
}
