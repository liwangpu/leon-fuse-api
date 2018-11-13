using ApiModel.Consts;
using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Models;
using BambooCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class WorkFlowRuleRepository : RepositoryBase<WorkFlowRule, WorkFlowRuleDTO>, IRepository<WorkFlowRule, WorkFlowRuleDTO>
    {
        public WorkFlowRuleRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }

        //public override async Task<WorkFlowRuleDTO> GetByIdAsync(string id)
        //{
        //    var data = await _DbContext.WorkFlowRules.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    if (!string.IsNullOrWhiteSpace(data.Keyword))
        //    {
        //        var detail=await _DbContext.WorkFlowRuleDetails.Where(x=>x.KeyWord)
        //    }
        //    return data.ToDTO();
        //}
    }
}
