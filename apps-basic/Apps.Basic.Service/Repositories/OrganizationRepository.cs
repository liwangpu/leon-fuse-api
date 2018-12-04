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
    public class OrganizationRepository : IRepository<Organization>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public OrganizationRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Organization data, string OrganizationId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string OrganizationId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanGetByIdAsync(string id, string OrganizationId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(Organization data, string OrganizationId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(Organization data, string OrganizationId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string OrganizationId)
        {
            throw new NotImplementedException();
        }

        public async Task<Organization> GetByIdAsync(string id, string OrganizationId)
        {
            var entity = await _Context.Organizations.Where(x => x.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<PagedData<Organization>> SimplePagedQueryAsync(PagingRequestModel model, string OrganizationId, Func<IQueryable<Organization>, Task<IQueryable<Organization>>> advanceQuery = null)
        {
            var query = _Context.Organizations.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize);
            return result;
        }

        public async Task UpdateAsync(Organization data, string OrganizationId)
        {
            throw new NotImplementedException();
        }

    }
}
