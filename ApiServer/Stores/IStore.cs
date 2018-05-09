using ApiModel;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public interface IStore<T>
             where T : class, IEntity, IDTOTransfer<IData>, new()
    {
        Task SatisfyCreate(string accid, T data, ModelStateDictionary modelState);
        Task SatisfyUpdate(string accid, T data, ModelStateDictionary modelState);
        Task<bool> CanCreate(string accid);
        Task<bool> CanUpdate(string accid, string id);
        Task<bool> CanDelete(string accid, string id);
        Task<bool> CanRead(string accid, string id);
        Task CreateAsync(string accid, T data);
        Task UpdateAsync(string accid, T data);
        Task DeleteAsync(string accid, string id);
        Task<PagedData<T>> SimplePagedQueryAsync(string accid, int page, int pageSize, string orderBy, bool desc, string search);
    }
}
