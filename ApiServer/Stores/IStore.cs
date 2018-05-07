using ApiModel;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    public interface IStore<T>
        where T : IData
    {
        //IQueryable<T> 
        Task<string> CanCreate(string accid, T data);
        Task<string> CanUpdate(string accid, T data);
        Task<string> CanDelete(string accid, string id);
        Task<string> CanRead(string accid, string id);
    }
}
