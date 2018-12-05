using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service.Repositories
{
    public class OrganizationTreeRepository : TreeBaseRepository<OrganizationTree>
    {
        #region 构造函数
        public OrganizationTreeRepository(AppDbContext context)
         : base(context)
        {
        }
        #endregion


    }
}
