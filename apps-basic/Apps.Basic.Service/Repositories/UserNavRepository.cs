using Apps.Base.Common.Interfaces;
using Apps.Base.Common.Models;
using Apps.Basic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Repositories
{
    public class UserNavRepository : IRepository<UserNav>
    {
        public async Task<string> CanCreateAsync(UserNav data, string accountId)
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

        public async Task<string> CanUpdateAsync(UserNav data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(UserNav data, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserNav> GetByIdAsync(string id, string accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedData<UserNav>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<UserNav>, Task<IQueryable<UserNav>>> advanceQuery = null)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(UserNav data, string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
