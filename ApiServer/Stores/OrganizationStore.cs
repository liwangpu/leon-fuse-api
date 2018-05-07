using ApiModel.Consts;
using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Services;
using BambooCommon;
using BambooCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        /**************** protected method ****************/

        #region OrganPermissionPipe 查询组织权限过滤
        /// <summary>
        /// 查询组织权限过滤
        /// </summary>
        /// <param name="query"></param>
        /// <param name="currentAcc"></param>
        protected void OrganPermissionPipe(ref IQueryable<Organization> query, Account currentAcc)
        {
            if (currentAcc.Type == AppConst.AccountType_SysAdmin)
            {

            }
            else if (currentAcc.Type == AppConst.AccountType_OrganAdmin)
            {
                query = query.Where(x => x.Id == currentAcc.OrganizationId);
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
        public async Task<PagedData<OrganizationDTO>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, Expression<Func<Organization, bool>> searchExpression = null)
        {
            try
            {
                var currentAcc = await _DbContext.Accounts.FindAsync(accid);
                var query = from it in _DbContext.Organizations
                            select it;
                _SearchExpressionPipe(ref query, searchExpression);
                _OrderByPipe(ref query, orderBy, desc);
                OrganPermissionPipe(ref query, currentAcc);
                var result = await query.SimplePaging(page, pageSize);
                if (result.Total > 0)
                    return new PagedData<OrganizationDTO>() { Data = result.Data.Select(x => x.ToDTO()), Total = result.Total, Page = page, Size = pageSize };
            }
            catch (Exception ex)
            {
                Logger.LogError("ProductStore SimplePagedQueryAsync", ex);
            }
            return new PagedData<OrganizationDTO>();
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrganizationDTO> GetByIdAsync(string accid, string id)
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
            return new OrganizationDTO();
        }
        #endregion

        #region CanCreate 判断组织信息是否符合存储规范
        /// <summary>
        /// 判断组织信息是否符合存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanCreate(string accid, Organization data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;

            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "组织名称", 50);
            }
            else
            {
                var exist = await _DbContext.Organizations.Where(x => x.Name == data.Name.Trim()).CountAsync();
                if (exist > 0)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "组织名称", data.Name.Trim());
            }

            if (string.IsNullOrWhiteSpace(data.Mail) || data.Mail.Length > 50)
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "邮箱", 50);
            }
            else
            {
                var exist = await _DbContext.Accounts.Where(x => x.Mail == data.Mail.Trim()).CountAsync();
                if (exist > 0)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "邮箱", data.Mail.Trim());
            }


            return string.Empty;
        }
        #endregion

        #region CanUpdate 判断组织信息是否符合更新规范
        /// <summary>
        /// 判断组织信息是否符合更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CanUpdate(string accid, Organization data)
        {
            var valid = _CanSave(accid, data);
            if (!string.IsNullOrWhiteSpace(valid))
                return valid;


            if (string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 50)
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "组织名称", 50);
            }
            else
            {
                var exist = await _DbContext.Organizations.Where(x => x.Name == data.Name.Trim() && x.Id != data.Id).CountAsync();
                if (exist > 0)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "组织名称", data.Name.Trim());
            }


            if (string.IsNullOrWhiteSpace(data.Mail) || data.Mail.Length > 50)
            {
                return string.Format(ValidityMessage.V_StringLengthRejectMsg, "邮箱", 50);
            }
            else
            {
                var organMailexist = await _DbContext.Organizations.Where(x => x.Mail == data.Mail.Trim() && x.Id != data.Id).CountAsync();
                var ownerMailExist = await _DbContext.Accounts.Where(x => x.Mail == data.Mail.Trim() && x.OrganizationId != data.Id).CountAsync();

                if (organMailexist > 0 && ownerMailExist > 0)
                    return string.Format(ValidityMessage.V_DuplicatedMsg, "邮箱", data.Mail.Trim());
            }


            return await Task.FromResult(string.Empty);
        }
        #endregion

        #region CanDelete 判断组织信息是否符合删除规范
        /// <summary>
        /// 判断组织信息是否符合删除规范
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
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var query = from it in _DbContext.Organizations
                        select it;
            OrganPermissionPipe(ref query, currentAcc);
            var result = await query.CountAsync(x => x.Id == id);
            if (result == 0)
                return ValidityMessage.V_NoPermissionReadMsg;
            return await Task.FromResult(string.Empty);
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
                department.OrganizationId = data.Id;
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

                account.ExpireTime = DateTime.Now.AddYears(10);
                account.ActivationTime = DateTime.UtcNow;
                account.Department = department;
                account.DepartmentId = department.Id;
                account.Organization = data;
                account.OrganizationId = data.Id;
                await _AccountStore.CreateAsync(accid, account);
                //将创建人和修改人改为该默认管理员
                account.Creator = account.Id;
                account.Modifier = account.Id;
                _DbContext.Accounts.Update(account);
                await _DbContext.SaveChangesAsync();
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

        #region GetOrganOwner 根据组织Id获取组织管理员信息
        /// <summary>
        /// 根据组织Id获取组织管理员信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<AccountDTO> GetOrganOwner(string organId)
        {
            var organ = await _DbContext.Organizations.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == organId);
            if (organ != null && organ.Owner != null)
                return organ.Owner.ToDTO();
            return new AccountDTO();
        }
        #endregion
    }
}
