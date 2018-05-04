using ApiModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    public interface IStore<T>
        where T : IEntity
    {
        //IQueryable<T> 
        Task<string> CanCreate(string accid, T data);
        Task<string> CanUpdate(string accid, T data);
        Task<string> CanDelete(string accid, string id);
        Task<string> CanRead(string accid, string id);
    }
}
