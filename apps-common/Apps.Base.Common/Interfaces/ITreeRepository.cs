using System.Threading.Tasks;

namespace Apps.Base.Common.Interfaces
{
    public interface ITreeRepository<T>
         where T : ITree
    {
        Task<T> GetNodeByObjId(string objId);
        Task CreateAsync(T data, string accountId);
        Task DeleteAsync(string id, string accountId);
    }
}
