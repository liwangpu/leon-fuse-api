using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using BambooCore;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductsController : ListableController<Product, ProductDTO>
    {
        private readonly ApiDbContext _context;

        #region 构造函数
        public ProductsController(ApiDbContext context)
        : base(new ProductStore(context))
        {
            _context = context;
        }
        #endregion

        #region Get 根据分页查询信息获取产品概要信息
        /// <summary>
        /// 根据分页查询信息获取产品概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryId"></param>
        /// <param name="classify"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<ProductDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model, string categoryId = "", bool classify = true)
        {
            var advanceQuery = new Func<IQueryable<Product>, Task<IQueryable<Product>>>(async (query) =>
            {
                if (classify)
                {
                    if (!string.IsNullOrWhiteSpace(categoryId))
                    {
                        var curCategoryTree = await _context.AssetCategoryTrees.FirstOrDefaultAsync(x => x.ObjId == categoryId);
                        //如果是根节点,把所有取出,不做分类过滤
                        if (curCategoryTree != null && curCategoryTree.LValue > 1)
                        {
                            var categoryQ = from it in _context.AssetCategoryTrees
                                            where it.NodeType == curCategoryTree.NodeType && it.OrganizationId == curCategoryTree.OrganizationId
                                            && it.LValue >= curCategoryTree.LValue && it.RValue <= curCategoryTree.RValue
                                            select it;
                            query = from it in query
                                    join cat in categoryQ on it.CategoryId equals cat.ObjId
                                    select it;
                        }
                    }
                }
                else
                {
                    query = query.Where(x => string.IsNullOrWhiteSpace(x.CategoryId));
                }
                return await Task.FromResult(query);
            });

            return await _GetPagingRequest(model, null, ResourceTypeEnum.Organizational, advanceQuery);
        }
        #endregion

        #region Get 根据id获取产品信息
        /// <summary>
        /// 根据id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id, ResourceTypeEnum.Organizational);
        }
        #endregion

        #region Post 新建产品信息
        /// <summary>
        /// 新建产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductCreateModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新产品信息
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductEditModel model)
        {
            var mapping = new Func<Product, Task<Product>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region BulkChangeCategory 批量修改产品分类信息
        /// <summary>
        /// 批量修改产品分类信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("BulkChangeCategory")]
        [ValidateModel]
        [HttpPut]
        public async Task<IActionResult> BulkChangeCategory([FromBody]BulkChangeCategoryModel model)
        {
            if (!ModelState.IsValid)
                return new ValidationFailedResult(ModelState);
            var existCategory = await _context.AssetCategories.CountAsync(x => x.Id == model.CategoryId) > 0;
            if (!existCategory)
            {
                ModelState.AddModelError("categoryId", "对应记录不存在");
                return new ValidationFailedResult(ModelState);
            }
            var idArr = model.Ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    for (int idx = idArr.Length - 1; idx >= 0; idx--)
                    {
                        var id = idArr[idx];
                        var refProduct = await _context.Products.FindAsync(id);
                        if (refProduct != null)
                        {
                            refProduct.CategoryId = model.CategoryId;
                            _context.Products.Update(refProduct);
                        }
                        else
                        {
                            ModelState.AddModelError("ProductId", "对应记录不存在");
                            return new ValidationFailedResult(ModelState);
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return Ok();
        }
        #endregion

        #region UploadFormFile Form表单方式上传一个文件
        /// <summary>
        /// FormData上传一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Route("ImportProductAndCategory")]
        [HttpPost]
        public async Task<IActionResult> ImportProductAndCategory(IFormFile file)
        {
            var accid = AuthMan.GetAccountId(this);
            var importOp = new Func<ProductAndCategoryCSV, Task<string>>(async (data) =>
            {
                //var mapProductCount = await _Store.get.CountAsync(x => x.&& x.Name.Trim() == data.ProductName.Trim());
                //var map
                //var refProduct = _context.Products.Where(x => x.Name.Trim() == data.ProductName.Trim());


                return await Task.FromResult(string.Empty);
            });
            return await _ImportRequest(file, importOp);
        }
        #endregion

        #region [ProductAndCategoryCSV] 批量修改产品列表Matedata
        /// <summary>
        /// 批量修改产品列表Matedata
        /// </summary>
        class ProductAndCategoryCSV : ImportMap<ProductAndCategoryCSV>
        {
            public ProductAndCategoryCSV()
            {
                Map(m => m.ProductName);
                Map(m => m.CategoryName);
            }

            public string ProductName { get; set; }
            public string CategoryName { get; set; }
        }
        #endregion
    }


}