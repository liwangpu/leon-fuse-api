using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Service.Contexts;

namespace Apps.MoreJee.Service.Repositories
{
    public class CategoryTreeRepository : TreeBaseRepository<AssetCategoryTree>
    {
        #region 构造函数
        public CategoryTreeRepository(AppDbContext context)
         : base(context)
        {
        }
        #endregion

    }
}
