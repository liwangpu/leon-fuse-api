using ApiModel;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public interface IStore<T, DTO>
             where T : class, IEntity, IDTOTransfer<DTO>, new()
                   where DTO : class, IData, new()
    {
        ApiDbContext DbContext { get; }
        Task SatisfyCreateAsync(string accid, T data, ModelStateDictionary modelState);
        Task SatisfyUpdateAsync(string accid, T data, ModelStateDictionary modelState);
        Task<bool> CanCreateAsync(string accid);
        Task<bool> CanUpdateAsync(string accid, string id);
        Task<bool> CanDeleteAsync(string accid, string id);
        Task<bool> CanReadAsync(string accid, string id);
        Task CreateAsync(string accid, T data);
        Task UpdateAsync(string accid, T data);
        Task DeleteAsync(string accid, string id);
        Task<PagedData<T>> SimplePagedQueryAsync(PagingRequestModel model, string accid, Func<IQueryable<T>, Task<IQueryable<T>>> advanceQuery = null);
        Task<bool> ExistAsync(string id, bool withInActive = false);
        Task<DTO> GetByIdAsync(string id);
        Task<T> _GetByIdAsync(string id);
        Task<IQueryable<T>> _GetPermissionData(string accid, DataOperateEnum dataOp, bool withInActive = false);
    }
}
