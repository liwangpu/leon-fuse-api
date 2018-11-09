using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<WorkFlow> _GetByIdAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var data = await _DbContext.WorkFlows.Include(x => x.WorkFlowItems).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data != null)
                    return data;
            }
            return new WorkFlow();
        }

        public override async Task<WorkFlowDTO> GetByIdAsync(string id)
        {
            var data = await _DbContext.WorkFlows.Include(x => x.WorkFlowItems).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data.WorkFlowItems != null && data.WorkFlowItems.Count > 0)
            {
                data.WorkFlowItems = data.WorkFlowItems.OrderBy(x => x.FlowGrade).ToList();
            }
            return data.ToDTO();
        }

    }
}
