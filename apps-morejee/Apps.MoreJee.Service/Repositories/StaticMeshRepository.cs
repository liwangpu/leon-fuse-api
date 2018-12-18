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
    public class StaticMeshRepository : IRepository<StaticMesh>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public StaticMeshRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(StaticMesh data, string accountId)
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

        public async Task<string> CanUpdateAsync(StaticMesh data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(StaticMesh data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.StaticMeshs.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<StaticMesh> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.StaticMeshs.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<StaticMesh>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<StaticMesh>, Task<IQueryable<StaticMesh>>> advanceQuery = null)
        {
            var query = _Context.StaticMeshs.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(StaticMesh data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.StaticMeshs.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
