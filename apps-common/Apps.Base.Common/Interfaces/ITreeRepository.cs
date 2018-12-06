using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Base.Common.Interfaces
{
    public interface ITreeRepository<T>
         where T : ITree
    {
        Task<IQueryable<T>> GetDescendantNode(string objId, List<string> nodeTypes, bool includeCurrentNode = false);
        Task<IQueryable<T>> GetAncestorNode(string objId, List<string> nodeTypes, bool includeCurrentNode = false);
        Task<T> GetNodeByObjId(string objId);
        Task CreateAsync(T data, string accountId);
        Task DeleteAsync(string id, string accountId);
    }
}
