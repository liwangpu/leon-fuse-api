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
    public class AreaTypeRepository : IRepository<AreaType>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public AreaTypeRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(AreaType data, string accountId)
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

        public async Task<string> CanUpdateAsync(AreaType data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(AreaType data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.AreaTypes.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            var entity = await _Context.AreaTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.ActiveFlag = AppConst.InActive;
                entity.Modifier = accountId;
                entity.ModifiedTime = DateTime.Now;
                _Context.AreaTypes.Update(entity);
                await _Context.SaveChangesAsync();
            }
        }

        public async Task<AreaType> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.AreaTypes.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<AreaType>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<AreaType>, Task<IQueryable<AreaType>>> advanceQuery = null)
        {
            var query = _Context.AreaTypes.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));
            query = query.Where(x => x.ActiveFlag == AppConst.Active);
            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(AreaType data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.AreaTypes.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
