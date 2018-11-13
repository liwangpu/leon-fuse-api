using ApiModel.Entities;
using ApiServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class NavigationRepository : RepositoryBase<Navigation, NavigationDTO>, IRepository<Navigation, NavigationDTO>
    {
        public NavigationRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep) : base(context, permissionTreeRep)
        {
        }
    }
}
