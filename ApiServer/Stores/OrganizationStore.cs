using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class OrganizationStore : StoreBase<Organization, OrganizationDTO>, IStore<Organization, OrganizationDTO>
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

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Organization data, ModelStateDictionary modelState)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            if (!string.IsNullOrWhiteSpace(data.Mail))
            {
                var organMailExist = await _DbContext.Organizations.CountAsync(x => x.Mail == data.Mail) > 0;
                if (organMailExist)
                    modelState.AddModelError("Mail", "该邮箱已经使用");
            }

            if (!string.IsNullOrWhiteSpace(data.Type))
            {
                if (currentAcc.Type == AppConst.AccountType_SysAdmin)
                {
                    if (data.Type != AppConst.OrganType_Brand)
                        modelState.AddModelError("Type", "您没有权限创建该组织类型");
                }
                else if (currentAcc.Type == AppConst.AccountType_BrandAdmin)
                {
                    if (!(data.Type == AppConst.OrganType_Partner || data.Type == AppConst.OrganType_Supplier))
                        modelState.AddModelError("Type", "您没有权限创建该组织类型");
                }
                else
                {
                    modelState.AddModelError("Type", "您没有权限创建该组织类型");
                }

            }
            else
            {
                modelState.AddModelError("Type", "组织类型不能为空");
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
        public async Task SatisfyUpdateAsync(string accid, Organization data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(data.Mail))
            {
                var organMailExist = await _DbContext.Organizations.CountAsync(x => x.Mail == data.Mail && x.Id != data.Id) > 0;
                if (organMailExist)
                    modelState.AddModelError("Mail", "该邮箱已经使用");
            }
        }
        #endregion

        #region CanCreateAsync 判断用户是否可以创建数据
        /// <summary>
        /// 判断用户是否可以创建数据
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public override async Task<bool> CanCreateAsync(string accid)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            if (currentAcc == null)
                return false;

            if (currentAcc.Type == AppConst.AccountType_SysAdmin || currentAcc.Type == AppConst.AccountType_BrandAdmin)
                return true;

            return false;
        }
        #endregion

        #region CreateAsync 新建组织信息
        /// <summary>
        /// 新建组织信息
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override async Task CreateAsync(string accid, Organization data)
        {
            var currentAcc = await _DbContext.Accounts.FindAsync(accid);
            var bCreateDefaultResource = false;
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data.Type == AppConst.OrganType_Partner || data.Type == AppConst.OrganType_Supplier)
                    {
                        data.ParentId = currentAcc.OrganizationId;
                    }


                    await base.CreateAsync(accid, data);
                    var otree = new PermissionTree();
                    otree.Id = GuidGen.NewGUID();
                    otree.Name = data.Name;
                    otree.OrganizationId = data.Id;
                    otree.NodeType = AppConst.S_NodeType_Organization;
                    otree.ObjId = data.Id;
                    //if (string.IsNullOrWhiteSpace(data.ParentId))
                    //{
                    await _PermissionTreeStore.AddNewNode(otree);
                    bCreateDefaultResource = true;
                    //}
                    //else
                    //{
                    //var parentOrganNode = _DbContext.PermissionTrees.Where(x => x.ObjId == data.ParentId).FirstOrDefault();
                    //if (parentOrganNode != null)
                    //{
                    //    otree.ParentId = parentOrganNode.Id;
                    //    otree.OrganizationId = data.ParentId;
                    //    await _PermissionTreeStore.AddChildNode(otree);
                    //}
                    //}
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw ex;//抛出异常让middleware捕获
                }
            }

            if (bCreateDefaultResource)
            {
                #region 创建默认部门
                var department = new Department();
                department.OrganizationId = data.Id;
                department.Organization = data;
                department.Name = data.Name;
                await _DepartmentStore.CreateAsync(accid, department);
                #endregion

                #region 创建默认管理员
                var account = new Account();
                if (data.Type == AppConst.OrganType_Brand)
                    account.Type = AppConst.AccountType_BrandAdmin;
                else if (data.Type == AppConst.OrganType_Partner)
                    account.Type = AppConst.AccountType_PartnerAdmin;
                else if (data.Type == AppConst.OrganType_Supplier)
                    account.Type = AppConst.AccountType_SupplierAdmin;
                else
                { }
                account.Name = "管理员";
                account.Mail = GuidGen.NewGUID();
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
        }
        #endregion

        public override async Task<IQueryable<Organization>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            var emptyQuery = Enumerable.Empty<Organization>().AsQueryable();
            var query = emptyQuery;

            var currentAcc = await _DbContext.Accounts.Include(x => x.Organization).FirstAsync(x => x.Id == accid);
            if (currentAcc == null)
                return query;

            //数据状态
            if (withInActive)
                query = _DbContext.Organizations;
            else
                query = _DbContext.Organizations.Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            #region 用户角色类型过滤(为高级用户角色定义查询,这些高级用户角色不走资源类型和数据操作类型过滤)
            {
                if (currentAcc.Type == AppConst.AccountType_SysAdmin)
                {
                    return query.Where(x => x.Type == AppConst.OrganType_Brand);
                }
                else if (currentAcc.Type == AppConst.AccountType_BrandAdmin)
                {
                    return query.Where(x => x.Type == AppConst.OrganType_Partner || x.Type == AppConst.OrganType_Supplier);
                }
                else
                { }
            }
            #endregion

            return emptyQuery;

        }

        #region GetOrganOwner 根据组织Id获取组织管理员信息
        /// <summary>
        /// 根据组织Id获取组织管理员信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        public async Task<IData> GetOrganOwner(string organId)
        {
            var organ = await _DbContext.Organizations.Include(x => x.Owner).Include(x => x.Owner.Department).FirstOrDefaultAsync(x => x.Id == organId);
            if (organ != null && organ.Owner != null)
                return organ.Owner.ToDTO();
            return new AccountDTO();
        }
        #endregion
    }
}
