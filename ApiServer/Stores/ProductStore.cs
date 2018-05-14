using ApiModel;
using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
namespace ApiServer.Stores
{
    public class ProductStore : StoreBase<Product, ProductDTO>, IStore<Product, ProductDTO>
    {
        #region 构造函数
        public ProductStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Product data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, Product data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region GetByIdAsync 根据Id返回实体DTO数据信息
        /// <summary>
        /// 根据Id返回实体DTO数据信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ProductDTO> GetByIdAsync(string id)
        {
            var data = await _GetByIdAsync(id);
            _DbContext.Entry(data).Collection(d => d.Specifications).Load();
            if (data.Specifications != null && data.Specifications.Count > 0)
            {
                for (int nidx = data.Specifications.Count - 1; nidx >= 0; nidx--)
                {
                    var spec = data.Specifications[nidx];
                    if (!string.IsNullOrWhiteSpace(spec.Icon))
                    {
                        var iconass = await _DbContext.Files.FindAsync(spec.Icon);
                        spec.IconFileAsset = iconass;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(data.CategoryId))
                data.AssetCategory = await _DbContext.AssetCategories.FindAsync(data.CategoryId);
            return data.ToDTO();
        }
        #endregion
    }
}
