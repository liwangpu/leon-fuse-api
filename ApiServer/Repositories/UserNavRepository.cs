using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Repositories
{
    public class UserNavRepository : RepositoryBase<UserNav, UserNavDTO>, IRepository<UserNav, UserNavDTO>
    {
        public UserNavRepository(ApiDbContext context, ITreeRepository<PermissionTree> permissionTreeRep)
            : base(context, permissionTreeRep)
        {
        }
    }
}
