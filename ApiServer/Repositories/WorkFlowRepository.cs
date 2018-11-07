using ApiModel.Entities;
using ApiServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class WorkFlowRepository : RepositoryBase<WorkFlow, WorkFlowDTO>, IRepository<WorkFlow, WorkFlowDTO>
    {
        public WorkFlowRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep) 
            : base(context, permissionTreeRep)
        {
        }



    }
}
