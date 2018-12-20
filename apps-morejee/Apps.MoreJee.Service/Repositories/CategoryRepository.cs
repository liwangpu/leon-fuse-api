using Apps.Base.Common;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Repositories
{
    public class CategoryRepository : IRepository<AssetCategory>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public CategoryRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(AssetCategory data, string accountId)
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

        public async Task<string> CanUpdateAsync(AssetCategory data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(AssetCategory data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            //校正index
            data.DisplayIndex = await _Context.AssetCategories.CountAsync(x => x.Type == data.Type && x.OrganizationId == data.OrganizationId && x.ParentId == data.ParentId);
            _Context.AssetCategories.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var entity = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _Context.AssetCategories.Remove(entity);
                await _Context.SaveChangesAsync();
            }
        }

        public async Task<AssetCategory> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<AssetCategory>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<AssetCategory>, Task<IQueryable<AssetCategory>>> advanceQuery = null)
        {
            var query = _Context.AssetCategories.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(AssetCategory data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.AssetCategories.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
