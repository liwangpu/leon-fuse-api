using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.FileSystem.Data.Entities;
using Apps.FileSystem.Service.Contexts;
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

        public async Task<string> CanUpdateAsync(FileAsset data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(FileAsset data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<FileAsset> GetByIdAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedData<FileAsset>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<FileAsset>, Task<IQueryable<FileAsset>>> advanceQuery = null)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(FileAsset data, string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
