using Apps.MoreJee.Data.Entities;
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
        /// <summary>
        /// 产品
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public DbSet<ProductSpec> ProductSpecs { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public DbSet<Layout> Layouts { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        public DbSet<Solution> Solutions { get; set; }
        /// <summary>
        /// 套餐
        /// </summary>
        public DbSet<Package> Packages { get; set; }
        /// <summary>
        /// 区域类型
        /// </summary>
        public DbSet<AreaType> AreaTypes { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public DbSet<ProductGroup> ProductGroups { get; set; }
        /// <summary>
        /// 产品替换组
        /// </summary>
        public DbSet<ProductReplaceGroup> ProductReplaceGroups { get; set; }
    }
}
