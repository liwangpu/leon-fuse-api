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
    public class AccountRepository : IRepository<Account>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public AccountRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanGetByIdAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanCreateAsync(Account data, string accountId)
        {

            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanUpdateAsync(Account data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<string> CanDeleteAsync(string id, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<PagedData<Account>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Account>, Task<IQueryable<Account>>> advanceQuery = null)
        {
            var query = _Context.Accounts.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            #region 组织过滤
            {
                var currentOrganId = await _Context.Accounts.Where(x => x.Id == accountId).Select(x => x.OrganizationId).FirstAsync();
                var currentOrgan = await _Context.Organizations.FirstAsync(x => x.Id == currentOrganId);
                query = query.Where(x => x.OrganizationId == currentOrganId && x.Id != currentOrgan.OwnerId);
            }
            #endregion

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "ModifiedTime", model.Desc);
            return result;
        }

        public async Task<Account> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public async Task CreateAsync(Account data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.Accounts.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.Accounts.Update(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var data = await _Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                _Context.Accounts.Remove(data);
                await _Context.SaveChangesAsync();
            }
        }

    }
}
