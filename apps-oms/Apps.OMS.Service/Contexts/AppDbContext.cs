using Apps.OMS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apps.OMS.Service.Contexts
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

        public DbSet<MemberRegistry> MemberRegistries { get; set; }
    }
}
