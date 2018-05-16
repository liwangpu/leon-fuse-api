using ApiModel;
using ApiModel.Consts;
using ApiModel.Entities;
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
            var organMailExist = await _DbContext.Organizations.CountAsync(x => x.Mail == data.Mail) > 0;
            if (organMailExist)
                modelState.AddModelError("Mail", "该邮箱已经使用");
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
            var organMailExist = await _DbContext.Organizations.CountAsync(x => x.Mail == data.Mail && x.Id != data.Id) > 0;
            if (organMailExist)
                modelState.AddModelError("Mail", "该邮箱已经使用");
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
            var bCreateDefaultResource = false;
            using (var tx = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    _DbContext.Organizations.Add(data);
                    await _DbContext.SaveChangesAsync();
                    var otree = new PermissionTree();
                    otree.Id = GuidGen.NewGUID();
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
                    throw ex;//抛出异常让middleware捕获
                }
            }

            if (bCreateDefaultResource)
            {
                #region 创建默认部门
                var department = new Department();
                department.Id = GuidGen.NewGUID();
                department.OrganizationId = data.Id;
                department.Organization = data;
                department.Name = data.Name;
                department.Creator = accid;
                department.Modifier = accid;
                await _DepartmentStore.CreateAsync(accid, department);
                #endregion

                #region 创建默认管理员
                var account = new Account();
                account.Id = GuidGen.NewGUID();
                account.Type = AppConst.AccountType_OrganAdmin;
                account.Name = "组织管理员";
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
