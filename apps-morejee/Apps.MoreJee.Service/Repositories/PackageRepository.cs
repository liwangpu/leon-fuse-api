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
    public class PackageRepository : IRepository<Package>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public PackageRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Package data, string accountId)
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

        public async Task<string> CanUpdateAsync(Package data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(Package data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.Packages.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var entity = await _Context.Packages.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.ActiveFlag = AppConst.InActive;
                entity.Modifier = accountId;
                entity.ModifiedTime = DateTime.Now;
                _Context.Packages.Update(entity);
                await _Context.SaveChangesAsync();
            }
        }

        public async Task<Package> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.Packages.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<Package>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Package>, Task<IQueryable<Package>>> advanceQuery = null)
        {
            var query = _Context.Packages.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));
            query = query.Where(x => x.ActiveFlag == AppConst.Active);
            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(Package data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.Packages.Update(data);
            await _Context.SaveChangesAsync();
        }

    }
}
