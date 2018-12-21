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
    public class ProductSpecRepository : IRepository<ProductSpec>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public ProductSpecRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(ProductSpec data, string accountId)
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

        public async Task<string> CanUpdateAsync(ProductSpec data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(ProductSpec data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.ProductSpecs.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductSpec> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.ProductSpecs.FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<PagedData<ProductSpec>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<ProductSpec>, Task<IQueryable<ProductSpec>>> advanceQuery = null)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ProductSpec data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.ProductSpecs.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
