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
        public async Task<string> CanCreate(string accid, Department data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "部门名称", 50);
            if (string.IsNullOrWhiteSpace(data.OrganizationId) || data.OrganizationId.Length > 50)
                return string.Format(ValidityMessage.V_RequiredRejectMsg, "组织编号");

            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanUpdate 判断部门信息是否符合更新规范
        /// <summary>
        /// 判断部门信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, Department data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "产品名称", 50);
            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断部门信息是否符合删除规范
        /// <summary>
        /// 判断部门信息是否符合删除规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CanDelete(string accid, string id)
        {
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

        #region CreateAsync 新建部门信息
        /// <summary>
        /// 新建部门信息
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

        #region UpdateAsync 更新部门信息
        /// <summary>
        /// 更新部门信息
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

        #region GetByOrgan 根据组织id获取部门信息
        /// <summary>
        /// 根据组织id获取部门信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<List<DepartmentDTO>> GetByOrgan(string organId)
        {
            var treeQ = from ps in _DbContext.PermissionTrees
                        where ps.OrganizationId == organId && ps.NodeType == AppConst.S_NodeType_Department
                        select ps;
            var query = from it in _DbContext.Departments
                        join ps in treeQ on it.Id equals ps.ObjId
                        select it;
            return await query.Select(x => x.ToDTO()).ToListAsync();
        } 
        #endregion
    }
}
