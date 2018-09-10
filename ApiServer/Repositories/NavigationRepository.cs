using ApiModel.Entities;
using ApiServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class NavigationRepository : TreeRepository<Navigation>, ITreeRepository<Navigation>
    {
        public NavigationRepository(ApiDbContext context)
            : base(context)
        {
        }
    }
}
