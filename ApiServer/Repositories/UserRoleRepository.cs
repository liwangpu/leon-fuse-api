using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole, UserRoleDTO>, IRepository<UserRole, UserRoleDTO>
    {

        public UserRoleRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {

        }

        public override async Task SatisfyCreateAsync(string accid, UserRole data, ModelStateDictionary modelState)
        {
            if (!string.IsNullOrWhiteSpace(data.Role))
            {
                var existName = await _DbContext.UserRoles.CountAsync(x => x.Name == data.Name && data.ActiveFlag == 1) > 0;
                if (existName)
                    modelState.AddModelError("Name", "该角色名称已经使用");
            }

            if (!string.IsNullOrWhiteSpace(data.Role))
            {
                var existRole = await _DbContext.UserRoles.CountAsync(x => x.Role == data.Role && data.ActiveFlag == 1) > 0;
                if (existRole)
                    modelState.AddModelError("Role", "该角色关键词已经使用");
            }


        }

        public override async Task SatisfyUpdateAsync(string accid, UserRole data, ModelStateDictionary modelState)
        {
            if (data.IsInner)
            {
                modelState.AddModelError("IsInner", "不能修改内置量角色信息");
            }
            if (!string.IsNullOrWhiteSpace(data.Role))
            {
                var exist = await _DbContext.UserRoles.CountAsync(x => x.Role == data.Role && data.ActiveFlag == 1 && x.Id != data.Id) > 0;
                if (exist)
                    modelState.AddModelError("Role", "该角色关键词已经使用");
            }
            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                var exist = await _DbContext.UserRoles.CountAsync(x => x.Name == data.Name && data.ActiveFlag == 1 && x.Id != data.Id) > 0;
                if (exist)
                    modelState.AddModelError("Name", "该角色名称已经使用");
            }

        }

        public override async Task SatisfyDeleteAsync(string accid, UserRole data, ModelStateDictionary modelState)
        {
            if (data.IsInner)
            {
                modelState.AddModelError("IsInner", "不能删除内置量角色信息");
                await Task.FromResult(string.Empty);
            }
        }

        public override async Task<IQueryable<UserRole>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false)
        {
            var emptyQuery = Enumerable.Empty<UserRole>().AsQueryable();
            var query = emptyQuery;

            var currentAcc = await _DbContext.Accounts.Select(x => new Account() { Id = x.Id, OrganizationId = x.OrganizationId, Type = x.Type }).FirstOrDefaultAsync(x => x.Id == accid);
            if (currentAcc == null)
                return query;

            //数据状态
            if (withInActive)
                query = _DbContext.Set<UserRole>();
            else
                query = _DbContext.Set<UserRole>().Where(x => x.ActiveFlag == AppConst.I_DataState_Active);




            return query;
        }

    }
}
