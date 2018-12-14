using Apps.FileSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apps.FileSystem.Service.Contexts
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

        /// <summary>
        /// 用户导航栏详细信息
        /// </summary>
        public DbSet<FileAsset> FileAssets { get; set; }
    }
}
