using Apps.Base.Common.Enums;
using Apps.Base.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Base.Common.Interfaces
{
    public interface IRepository<T>
        where T : IData
    {
        Task<string> CanGetByIdAsync(string id, string accountId);
        Task<string> CanCreateAsync(T data, string accountId);
        Task<string> CanUpdateAsync(T data, string accountId);
        Task<string> CanDeleteAsync(string id, string accountId);
        Task<T> GetByIdAsync(string id, string accountId);
        Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accountId, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null);
        Task CreateAsync(T data, string accountId);
        Task UpdateAsync(T data, string accountId);
        Task DeleteAsync(string id, string accountId);
    }
}
