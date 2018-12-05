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
    public class UserNavRepository : IRepository<UserNav>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public UserNavRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(UserNav data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(UserNav data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(UserNav data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            _Context.UserNavs.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserNav> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.UserNavs.Include(x=>x.UserNavDetails).FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<UserNav>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<UserNav>, Task<IQueryable<UserNav>>> advanceQuery = null)
        {
            var query = _Context.UserNavs.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(UserNav data, string accountId)
        {
            _Context.UserNavs.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
