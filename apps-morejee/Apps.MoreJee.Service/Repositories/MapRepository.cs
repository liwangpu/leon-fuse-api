using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Service.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Repositories
{
    public class MapRepository : IRepository<Map>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public MapRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(Map data, string accountId)
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

        public async Task<string> CanUpdateAsync(Map data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Map data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<Map> GetByIdAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedData<Map>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<Map>, Task<IQueryable<Map>>> advanceQuery = null)
        {
            var query = _Context.Maps.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(Map data, string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
