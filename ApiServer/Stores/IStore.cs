using ApiModel;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    public interface IStore<T>
        where T : IEntity
    {
        Task<List<string>> CanCreate(string accid, T data);
        Task<List<string>> CanUpdate(string accid, T data);
        Task<List<string>> CanDelete(string accid, string id);
        Task<List<string>> CanRead(string accid, string id);
    }
}
