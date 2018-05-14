using ApiModel;
using ApiModel.Enums;
using ApiServer.Models;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public interface IStore<T, DTO>
             where T : class, IEntity, IDTOTransfer<DTO>, new()
                   where DTO : class, IData, new()
    {
        Task SatisfyCreateAsync(string accid, T data, ModelStateDictionary modelState);
        Task SatisfyUpdateAsync(string accid, T data, ModelStateDictionary modelState);
        Task<bool> CanCreateAsync(string accid, ResourceTypeEnum resType = ResourceTypeEnum.Personal);
        Task<bool> CanUpdateAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal);
        Task<bool> CanDeleteAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal);
        Task<bool> CanReadAsync(string accid, string id, ResourceTypeEnum resType = ResourceTypeEnum.Personal);
        Task CreateAsync(string accid, T data);
        Task UpdateAsync(string accid, T data);
        Task DeleteAsync(string accid, string id);
        Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid, ResourceTypeEnum resType = ResourceTypeEnum.Personal);
        Task<bool> ExistAsync(string id);
        Task<DTO> GetByIdAsync(string id);
        Task<T> _GetByIdAsync(string id);
    }
}
