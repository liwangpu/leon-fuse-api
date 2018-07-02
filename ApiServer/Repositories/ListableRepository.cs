using ApiModel;
using ApiModel.Entities;
using ApiServer.Data;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class ListableRepository<T, DTO> : RepositoryBase<T, DTO>,IRepository<T,DTO>
     where T : class, IListable, IDTOTransfer<DTO>, new()
    where DTO : class, IData, new()
    {
        public ListableRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {

        }

    }
}
