using Apps.Base.Common;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Repositories
{
    public class SolutionRepository : IRepository<Solution>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public SolutionRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Solution data, string accountId)
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

        public async Task<string> CanUpdateAsync(Solution data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(Solution data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.Solutions.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var entity = await _Context.Solutions.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.ActiveFlag = AppConst.InActive;
                entity.Modifier = accountId;
                entity.ModifiedTime = DateTime.Now;
                _Context.Solutions.Update(entity);
                await _Context.SaveChangesAsync();
            }
        }

        public async Task<Solution> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Solutions.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<Solution>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Solution>, Task<IQueryable<Solution>>> advanceQuery = null)
        {
            var query = _Context.Solutions.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));
            query = query.Where(x => x.ActiveFlag == AppConst.Active);
            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(Solution data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.Solutions.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
