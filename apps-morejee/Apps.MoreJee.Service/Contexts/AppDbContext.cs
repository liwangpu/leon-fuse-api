﻿using Apps.MoreJee.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apps.MoreJee.Service.Contexts
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
        /// 场景
        /// </summary>
        public DbSet<Map> Maps { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public DbSet<Material> Materials { get; set; }
        /// <summary>
        /// 模型
        /// </summary>
        public DbSet<StaticMesh> StaticMeshs { get; set; }
        /// <summary>
        /// 资源分类
        /// </summary>
        public DbSet<AssetCategory> AssetCategories { get; set; }
        /// <summary>
        /// 资源分类树
        /// </summary>
        public DbSet<AssetCategoryTree> AssetCategoryTrees { get; set; }

    }
}
