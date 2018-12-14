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

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        /// <summary>
        /// 用户附属角色
        /// </summary>
        public DbSet<AdditionRole> AdditionRoles { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }
        /// <summary>
        /// 组织类型
        /// </summary>
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        /// <summary>
        /// 组织树
        /// </summary>
        public DbSet<OrganizationTree> OrganizationTrees { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public DbSet<Department> Departments { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }
        /// <summary>
        /// 导航栏项
        /// </summary>
        public DbSet<Navigation> Navigations { get; set; }
        /// <summary>
        /// 用户导航栏
        /// </summary>
        public DbSet<UserNav> UserNavs { get; set; }
        /// <summary>
        /// 用户导航栏详细信息
        /// </summary>
        public DbSet<UserNavDetail> UserNavDetails { get; set; }
    }
}
