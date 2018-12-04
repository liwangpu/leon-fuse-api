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
    public class OrganizationTypeRepository : IRepository<OrganizationType>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public OrganizationTypeRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(OrganizationType data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(OrganizationType data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(OrganizationType data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrganizationType> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.OrganizationTypes.Where(x => x.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<PagedData<OrganizationType>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<OrganizationType>, Task<IQueryable<OrganizationType>>> advanceQuery = null)
        {
            var query = _Context.OrganizationTypes.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize);
            return result;
        }

        public async Task UpdateAsync(OrganizationType data, string accountId)
        {
            throw new NotImplementedException();
        }
   
    }
}
