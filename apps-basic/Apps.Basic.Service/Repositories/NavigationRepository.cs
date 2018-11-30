using Apps.Base.Common.Interfaces;
using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Apps.Base.Common.Models;
using Apps.Base.Common.Enums;
using Apps.Base.Common;


namespace Apps.Basic.Service.Repositories
{
    public class NavigationRepository : IRepository<Navigation>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public NavigationRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Navigation data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanUpdateAsync(Navigation data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Navigation data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<Navigation> GetByIdAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedData<Navigation>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Navigation>, Task<IQueryable<Navigation>>> advanceQuery = null)
        {
            var query = _Context.Navigations.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize);
            return result;
        }

        public async Task UpdateAsync(Navigation data, string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
