using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ApiModel;
using BambooCommon;

namespace ApiServer.Data
{

    public class ApiDbContext : DbContext
    {
        //global------------------------------------------------------------------

        /// <summary>
        /// 全局设置
        /// </summary>
        public DbSet<SettingsItem> Settings { get; set; }

        //account-----------------------------------------------------------------

        /// <summary>
        /// 账号
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        //assets------------------------------------------------------------------

        /// <summary>
        /// 文件夹
        /// </summary>
        public DbSet<AssetFolder> AssetFolders { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public DbSet<AssetCategory> AssetCategories { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public DbSet<AssetTag> AssetTags { get; set; }
        /// <summary>
        /// 客户端资源
        /// </summary>
        public DbSet<ClientAsset> ClientAssets { get; set; }
        /// <summary>
        /// 文件资源
        /// </summary>
        public DbSet<FileAsset> Files { get; set; }

        //production--------------------------------------------------------------

        /// <summary>
        /// 地图
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        public DbSet<Solution> Solutions { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public DbSet<Layout> Layouts { get; set; }
        /// <summary>
        /// 墙体装饰，踢脚线，顶角线等
        /// </summary>
        public DbSet<Skirting> Skirtings { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public DbSet<PermissionItem> Permissions { get; set; }

        /// <summary>
        /// 模型
        /// </summary>
        public DbSet<StaticMesh> StaticMeshs { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        //------------------------------------------------------------------------

        public ApiDbContext() { }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(@"Server=localhost;Database=dmz;User Id=postgres;Password=root");
            //optionsBuilder.UseSqlServer(@"Data Source=qds116703363.my3w.com;Initial Catalog=qds116703363_db;Persist Security Info=True;User ID=qds116703363;Password=AAaa123456");
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<PermissionItem>().HasIndex(d => new { d.AccountId, d.ResId, d.ResType });
        }
    }
}
