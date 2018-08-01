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

namespace ApiServer.Repositories
{
    public class ResourceRepositoryBase<T, DTO> : ListableRepository<T, DTO>, IRepository<T, DTO>
     where T : class, IListable, IDTOTransfer<DTO>, new()
    where DTO : class, IData, new()
    {
        public virtual int ResType { get; }

        #region 构造函数
        public ResourceRepositoryBase(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
        : base(context, permissionTreeRep)
        {

        }
        #endregion

        public async override Task<IQueryable<T>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            IQueryable<T> query;

            var currentAcc = await _DbContext.Accounts.Select(x => new Account() { Id = x.Id, OrganizationId = x.OrganizationId, Type = x.Type }).FirstOrDefaultAsync(x => x.Id == accid);

            //数据状态
            if (withInActive)
                query = _DbContext.Set<T>();
            else
                query = _DbContext.Set<T>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);

            //超级管理员和品牌组织不走权限判断
            if (currentAcc.Type == AppConst.AccountType_SysAdmin || currentAcc.Type == AppConst.AccountType_BrandAdmin)
                return await Task.FromResult(query);
            else if (currentAcc.Type == AppConst.AccountType_BrandMember)
            {
                if (dataOp == DataOperateEnum.Update)
                    return await Task.FromResult(query.Take(0));
                else if (dataOp == DataOperateEnum.Delete)
                    return await Task.FromResult(query.Take(0));
                return await Task.FromResult(query);
            }
            else { }


            IQueryable<ResourcePermission> permissions;
            permissions = _DbContext.ResourcePermissions.Where(x => x.OrganizationId == currentAcc.OrganizationId && x.ResType == ResType);
            if (dataOp == DataOperateEnum.Retrieve)
                permissions = permissions.Where(x => x.OpRetrieve == 1);
            else if (dataOp == DataOperateEnum.Update)
                permissions = permissions.Where(x => x.OpUpdate == 1);
            else if (dataOp == DataOperateEnum.Delete)
                permissions = permissions.Where(x => x.OpDelete == 1);
            else
            { }

            query = from it in query
                    join ps in permissions on it.Id equals ps.ResId
                    select it;



            return await Task.FromResult(query);
        }
    }
}
