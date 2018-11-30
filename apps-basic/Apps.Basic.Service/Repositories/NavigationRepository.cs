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
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            var entity = await _Context.Navigations.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return "记录不存在";
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(Navigation data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(Navigation data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            _Context.Navigations.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var data = await _Context.Navigations.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                _Context.Navigations.Remove(data);
                await _Context.SaveChangesAsync();
            }
        }

        public async Task<Navigation> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Navigations.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
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
            _Context.Navigations.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
