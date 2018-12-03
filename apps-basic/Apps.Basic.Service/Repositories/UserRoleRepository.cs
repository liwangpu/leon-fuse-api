using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Repositories
{
    public class UserRoleRepository : IRepository<UserRole>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public UserRoleRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(UserRole data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(UserRole data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(UserRole data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.UserRoles.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<UserRole>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<UserRole>, Task<IQueryable<UserRole>>> advanceQuery = null)
        {
            var query = _Context.UserRoles.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize);
            return result;
        }

        public async Task UpdateAsync(UserRole data, string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
