using ApiModel;
using ApiServer.Models;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public interface IStore<T>
             where T : class, IEntity, IDTOTransfer<IData>, new()
    {
        Task SatisfyCreateAsync(string accid, T data, ModelStateDictionary modelState);
        Task SatisfyUpdateAsync(string accid, T data, ModelStateDictionary modelState);
        Task<bool> CanCreateAsync(string accid);
        Task<bool> CanUpdateAsync(string accid, string id);
        Task<bool> CanDeleteAsync(string accid, string id);
        Task<bool> CanReadAsync(string accid, string id);
        Task CreateAsync(string accid, T data);
        Task UpdateAsync(string accid, T data);
        Task DeleteAsync(string accid, string id);
        Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid);
        Task<bool> ExistAsync(string id);
        Task<IData> GetByIdAsync(string id);
        Task<T> _GetByIdAsync(string id);
    }
}
