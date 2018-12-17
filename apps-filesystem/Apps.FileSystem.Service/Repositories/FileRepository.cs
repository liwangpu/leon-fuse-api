using Apps.Base.Common;
using Apps.Base.Common.Consts;
using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.FileSystem.Data.Entities;
using Apps.FileSystem.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.FileSystem.Service.Repositories
{
    public class FileRepository : IRepository<FileAsset>
    {
        protected readonly AppDbContext _Context;

        #region 构造函数
        public FileRepository(AppDbContext context)
        {
            _Context = context;
        }
        #endregion

        public async Task<string> CanCreateAsync(FileAsset data, string accountId)
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

        public async Task<string> CanUpdateAsync(FileAsset data, string accountId)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task CreateAsync(FileAsset data, string accountId)
        {
            data.Id = GuidGen.NewGUID();
            data.ActiveFlag = AppConst.Active;
            data.Creator = accountId;
            data.Modifier = accountId;
            data.CreatedTime = DateTime.Now;
            data.ModifiedTime = data.CreatedTime;
            _Context.FileAssets.Add(data);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<FileAsset> GetByIdAsync(string id, string accountId)
        {
            var entity = await _Context.FileAssets.Where(x => x.Id == id).FirstOrDefaultAsync();
            return entity;
        }

        public async Task<PagedData<FileAsset>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<FileAsset>, Task<IQueryable<FileAsset>>> advanceQuery = null)
        {
            var query = _Context.FileAssets.AsQueryable();
            if (advanceQuery != null)
                query = await advanceQuery(query);
            //关键词过滤查询
            if (!string.IsNullOrWhiteSpace(model.Search))
                query = query.Where(d => d.Name.Contains(model.Search));

            var result = await query.SimplePaging(model.Page, model.PageSize, model.OrderBy, "Name", model.Desc);
            return result;
        }

        public async Task UpdateAsync(FileAsset data, string accountId)
        {
            data.Modifier = accountId;
            data.ModifiedTime = data.CreatedTime;
            _Context.FileAssets.Update(data);
            await _Context.SaveChangesAsync();
        }
    }
}
