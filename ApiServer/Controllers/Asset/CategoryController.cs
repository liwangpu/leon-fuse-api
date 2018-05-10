using ApiModel.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class CategoryController : Controller
    {
        Services.CategoryMan catman;

        public CategoryController(Data.ApiDbContext context)
        {
            catman = new Services.CategoryMan(context);
        }

        /// <summary>
        /// 获取整个类型(product, material)下的所有分类信息，已经整理成一个树结构。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AssetCategoryDTO> Get(string type, string organId)
        {
            if (string.IsNullOrEmpty(type))
                type = "product";
            return await catman.GetCategoryAsync(type, organId);
        }

        [Route("all")]
        [HttpGet]
        public async Task<AssetCategoryPack> GetAll(string organId)
        {
            AssetCategoryPack pack = new AssetCategoryPack();
            pack.Categories = new List<AssetCategoryDTO>();
            AssetCategoryDTO cat = null;
            cat = await catman.GetCategoryAsync("product", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await catman.GetCategoryAsync("material", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await catman.GetCategoryAsync("package", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await catman.GetCategoryAsync("order", organId); if (cat != null) pack.Categories.Add(cat);
            return pack;
        }

        #region GetFlat 获取扁平结构的分类信息
        /// <summary>
        /// 获取扁平结构的分类信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        [Route("Flat")]
        [HttpGet]
        public async Task<List<AssetCategoryDTO>> GetFlat(string type, string organId)
        {
            return await catman.GetFlatCategory(type, organId);
        }
        #endregion

        /// <summary>
        /// 创建一个分类。必须指定一个父级ID，不能主动创建根节点，根节点在get时会自动创建。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(AssetCategoryDTO))]
        public async Task<IActionResult> Post([FromBody]AssetCategoryDTO value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            AssetCategoryDTO result = await catman.CreateAsync(value);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// 修改一个分类的基本信息， 名称，描述，图标。其他属性会被忽略，比如子分类，显示位置。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(AssetCategoryDTO))]
        public async Task<IActionResult> Put([FromBody]AssetCategoryDTO value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            AssetCategoryDTO result = await catman.ModifyAsync(value);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// 删除一个分类，注意只能删除空分类。此分类下还有子分类或者还有资源，需要先转移或者删除后才能删除。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string type, string id)
        {
            string result = await catman.DeleteAsync(type, id);
            if (result == "")
                return Ok();
            return BadRequest(result);
        }

        /// <summary>
        /// 将一个分类移动到另外一个分类下作为其子分类。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Route("Move")]
        [HttpPost]
        public async Task<IActionResult> Move(string type, string id, string targetId)
        {
            string result = await catman.MoveAsync(type, id, targetId);
            if (result == "")
                return Ok();
            return BadRequest(result);
        }

        /// <summary>
        /// 将一个分类下的所有资源转移到另外一个分类中，目标分类必须是没有子分类的分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Route("Transfer")]
        [HttpPost]
        public async Task<IActionResult> Transfer(string type, string id, string targetId)
        {
            string result = await catman.TransferAsync(type, id, targetId);
            if (result == "")
                return Ok();
            return BadRequest(result);
        }

        /// <summary>
        /// 设置分类在父级分类中的显示顺序，index从0到childrencount -1，会自动纠正非法index。
        /// 返回此分类的父级分类以及兄弟分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [Route("DisplayIndex")]
        [HttpPost]
        [Produces(typeof(AssetCategoryDTO))]
        public async Task<IActionResult> DisplayIndex(string type, string id, int index)
        {
            var result = await catman.SetDisplayIndex(type, id, index);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}
