using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apps.Basic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apps.Basic.Service.Contexts
{
    public class AppDbContext : DbContext
    {
        #region 构造函数
        protected AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        #endregion

        public DbSet<Account> Accounts { get; set; }
    }
}
