using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class CategoryController : CommonController<AssetCategory, AssetCategoryDTO>
    {

        #region 构造函数
        public CategoryController(ApiDbContext context)
        : base(new AssetCategoryStore(context))
        {
        }
        #endregion

        #region Get 获取整个类型(product, material)下的所有分类信息，已经整理成一个树结构
        /// <summary>
        /// 获取整个类型(product, material)下的所有分类信息，已经整理成一个树结构
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AssetCategoryDTO> Get(string type, string organId)
        {
            if (string.IsNullOrWhiteSpace(organId))
                organId = await _GetCurrentUserOrganId();

            if (string.IsNullOrEmpty(type))
                type = "product";
            return await (_Store as AssetCategoryStore).GetCategoryAsync(type, organId);
        }
        #endregion

        #region GetAll 获取所有分类信息
        /// <summary>
        /// 获取所有分类信息
        /// </summary>
        /// <param name="organId"></param>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public async Task<AssetCategoryPack> GetAll(string organId)
        {
            if (string.IsNullOrWhiteSpace(organId))
                organId = await _GetCurrentUserOrganId();

            AssetCategoryPack pack = new AssetCategoryPack();
            pack.Categories = new List<AssetCategoryDTO>();
            AssetCategoryDTO cat = null;
            cat = await (_Store as AssetCategoryStore).GetCategoryAsync("product", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await (_Store as AssetCategoryStore).GetCategoryAsync("material", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await (_Store as AssetCategoryStore).GetCategoryAsync("package", organId); if (cat != null) pack.Categories.Add(cat);
            cat = await (_Store as AssetCategoryStore).GetCategoryAsync("order", organId); if (cat != null) pack.Categories.Add(cat);
            return pack;
        }
        #endregion

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
            if (string.IsNullOrWhiteSpace(organId))
                organId = await _GetCurrentUserOrganId();

            return await (_Store as AssetCategoryStore).GetFlatCategory(type, organId);
        }
        #endregion

        #region Post 创建一个分类
        /// <summary>
        /// 创建一个分类
        /// 必须指定一个父级ID，不能主动创建根节点，根节点在get时会自动创建。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(AssetCategoryDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]AssetCategoryCreateModel model)
        {
            var mapping = new Func<AssetCategory, Task<AssetCategory>>(async (entity) =>
            {
                if (string.IsNullOrWhiteSpace(model.OrganizationId))
                    model.OrganizationId = await _GetCurrentUserOrganId();
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ParentId = model.ParentId;
                if (!string.IsNullOrWhiteSpace(model.ParentId))
                    entity.Type = (await _Store.DbContext.AssetCategories.FirstAsync(d => d.Id == model.ParentId)).Type;
                else
                    entity.Type = model.Type;
                entity.OrganizationId = model.OrganizationId;
                entity.DisplayIndex = await _Store.DbContext.AssetCategories.Where(d => d.ParentId == model.ParentId).CountAsync();
                return entity;
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 修改一个分类的基本信息
        /// <summary>
        /// 修改一个分类的基本信息
        /// 名称，描述，图标。其他属性会被忽略，比如子分类，显示位置。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(MapDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]AssetCategoryEditModel model)
        {
            var mapping = new Func<AssetCategory, Task<AssetCategory>>(async (entity) =>
            {
                if (string.IsNullOrWhiteSpace(model.OrganizationId))
                    model.OrganizationId = await _GetCurrentUserOrganId();
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ParentId = model.ParentId;
                entity.OrganizationId = model.OrganizationId;
                return entity;
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Move 将一个分类移动到另外一个分类下作为其子分类
        /// <summary>
        /// 将一个分类移动到另外一个分类下作为其子分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [Route("Move")]
        [HttpPost]
        public async Task<IActionResult> Move(string type, string id, string targetId)
        {
            string result = await (_Store as AssetCategoryStore).MoveAsync(type, id, targetId);
            if (result == "")
                return Ok();
            return BadRequest(result);
        }
        #endregion

        #region Transfer 将一个分类下的所有资源转移到另外一个分类中，目标分类必须是没有子分类的分类
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
            string result = await (_Store as AssetCategoryStore).TransferAsync(type, id, targetId);
            if (result == "")
                return Ok();
            return BadRequest(result);
        }
        #endregion

        #region DisplayIndex 设置分类在父级分类中的显示顺序
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
            var result = await (_Store as AssetCategoryStore).SetDisplayIndex(type, id, index);
            if (result == null)
                return BadRequest();

            return Ok(result);
        } 
        #endregion


    }
}
