﻿using Apps.Base.Common;
using Apps.Base.Common.Attributes;
using Apps.Base.Common.Controllers;
using Apps.Base.Common.Interfaces;
using Apps.MoreJee.Data.Entities;
using Apps.MoreJee.Export.DTOs;
using Apps.MoreJee.Export.Models;
using Apps.MoreJee.Service.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.MoreJee.Service.Controllers
{
    /// <summary>
    /// 资源分类控制器
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ServiceBaseController<AssetCategory>
    {
        protected const string Product_Category = "product";
        protected const string Material_Category = "material";
        protected const string ProductGroup_Category = "product-group";
        protected AppConfig _AppConfig { get; }
        protected AppDbContext _Context { get; }
        protected ITreeRepository<AssetCategoryTree> _CategoryTreeRepository { get; }

        #region 构造函数
        public CategoryController(IRepository<AssetCategory> repository, ITreeRepository<AssetCategoryTree> treeRepository, AppDbContext context, IOptions<AppConfig> settingsOptions)
            : base(repository)
        {
            _CategoryTreeRepository = treeRepository;
            _Context = context;
            _AppConfig = settingsOptions.Value;
        }
        #endregion

        #region _FindCategoryChildren 获取下级分类信息
        /// <summary>
        /// 获取下级分类信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parent"></param>
        protected void _FindCategoryChildren(LinkedList<AssetCategory> list, CategoryDTO parent)
        {
            if (parent.Children == null)
                parent.Children = new List<CategoryDTO>();

            var node = list.First;
            while (node != null)
            {
                var next = node.Next;

                var item = node.Value;
                if (item.ParentId == parent.Id)
                {
                    var child = new CategoryDTO();
                    child.Id = item.Id;
                    child.Name = item.Name;
                    child.Description = item.Description;
                    child.Type = item.Type;
                    child.DisplayIndex = item.DisplayIndex;
                    child.Creator = item.Creator;
                    child.Modifier = item.Modifier;
                    child.CreatedTime = item.CreatedTime;
                    child.ModifiedTime = item.ModifiedTime;
                    parent.Children.Add(child);
                    list.Remove(node);
                }
                node = next;
            }

            parent.Children = parent.Children.OrderBy(x => x.DisplayIndex).ToList();

            foreach (var item in parent.Children)
            {
                _FindCategoryChildren(list, item);
            }
        }
        #endregion

        #region _GetParentCategoryAndSameLevelChildren 根据父节点Id获取父节点下第一级子节点分类
        /// <summary>
        /// 根据父节点Id获取父节点下第一级子节点分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected async Task<CategoryDTO> _GetParentCategoryAndSameLevelChildren(string parentId)
        {
            var parentCat = await _Context.AssetCategories.FirstAsync(d => d.Id == parentId);
            if (parentCat != null)
            {
                var dto = new CategoryDTO();
                dto.Id = parentCat.Id;
                dto.Name = parentCat.Name;
                dto.Description = parentCat.Description;
                dto.Type = parentCat.Type;
                dto.DisplayIndex = parentCat.DisplayIndex;
                dto.Creator = parentCat.Creator;
                dto.Modifier = parentCat.Modifier;
                dto.CreatedTime = parentCat.CreatedTime;
                dto.ModifiedTime = parentCat.ModifiedTime;

                dto.Children = new List<CategoryDTO>();
                var sameLevelCats = await _Context.AssetCategories.Where(d => d.ParentId == parentId).Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                    DisplayIndex = x.DisplayIndex,
                    Creator = x.Creator,
                    Modifier = x.Modifier,
                    CreatedTime = x.CreatedTime,
                    ModifiedTime = x.ModifiedTime
                }).OrderBy(x => x.DisplayIndex).ToListAsync();
                dto.Children.AddRange(sameLevelCats);
                dto.Children = dto.Children.OrderBy(x => x.DisplayIndex).ToList();
                return dto;
            }
            return new CategoryDTO();
        }
        #endregion

        #region Get 根据Id获取场景信息
        /// <summary>
        /// 根据Id获取场景信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async override Task<IActionResult> Get(string id)
        {
            //var accountMicroService = new AccountMicroService(_AppConfig.APIGatewayServer, Token);
            //var fileMicroServer = new FileMicroService(_AppConfig.APIGatewayServer, Token);

            var toDTO = new Func<AssetCategory, Task<CategoryDTO>>(async (entity) =>
            {
                var dto = new CategoryDTO();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Creator = entity.Creator;
                dto.Modifier = entity.Modifier;
                dto.CreatedTime = entity.CreatedTime;
                dto.ModifiedTime = entity.ModifiedTime;
                dto.OrganizationId = entity.OrganizationId;
                dto.Type = entity.Type;
                dto.DisplayIndex = entity.DisplayIndex;
                dto.ParentId = entity.ParentId;

                //await accountMicroService.GetNameByIds(entity.Creator, entity.Modifier, (creatorName, modifierName) =>
                //{
                //    dto.CreatorName = creatorName;
                //    dto.ModifierName = modifierName;
                //});
                //await fileMicroServer.GetUrlById(entity.Icon, (url) =>
                //{
                //    dto.Icon = url;
                //});
                return await Task.FromResult(dto);
            });
            return await _GetByIdRequest(id, toDTO);
        }
        #endregion

        #region GetByType 获取整个类型(product, material)下的所有分类信息，已经整理成一个树结构
        /// <summary>
        /// 获取整个类型(product, material)下的所有分类信息，已经整理成一个树结构
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async Task<CategoryDTO> GetByType(string type)
        {
            //标准化类型字段
            type = string.IsNullOrWhiteSpace(type) ? string.Empty : type.ToLower();
            var organId = CurrentAccountOrganizationId;
            _Context.AssetCategories.Where(x => x.Type == type.ToLower() && x.OrganizationId == organId).ToList();

            #region 创建默认分类
            {
                var existRoot = await _Context.AssetCategories.AnyAsync(x => x.Type == type && x.OrganizationId == CurrentAccountOrganizationId && x.IsRoot == true);
                if (!existRoot)
                {
                    //创建分类
                    var defaultCategory = new AssetCategory();
                    defaultCategory.Name = "Auto Create Category";
                    defaultCategory.Type = type;
                    defaultCategory.IsRoot = true;
                    defaultCategory.OrganizationId = CurrentAccountOrganizationId;
                    await _Repository.CreateAsync(defaultCategory, CurrentAccountId);

                    //创建分类树
                    var categoryTree = new AssetCategoryTree();
                    categoryTree.ObjId = defaultCategory.Id;
                    categoryTree.NodeType = type;
                    categoryTree.OrganizationId = defaultCategory.OrganizationId;
                    await _CategoryTreeRepository.CreateAsync(categoryTree, CurrentAccountId);
                }
            }
            #endregion

            var cats = _Context.AssetCategories.Where(x => x.Type == type && x.OrganizationId == organId).ToList();

            var rootDTO = cats.Where(x => x.IsRoot == true).Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Type = x.Type,
                DisplayIndex = x.DisplayIndex,
                Creator = x.Creator,
                Modifier = x.Modifier,
                CreatedTime = x.CreatedTime,
                ModifiedTime = x.ModifiedTime
            }).First();

            var linkedList = new LinkedList<AssetCategory>();
            foreach (var item in cats)
                linkedList.AddLast(item);
            _FindCategoryChildren(linkedList, rootDTO);

            return rootDTO;
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
        [ProducesResponseType(typeof(AssetCategoryPack), 200)]
        public async Task<AssetCategoryPack> GetAll(string organId)
        {
            var pack = new AssetCategoryPack();
            pack.Categories = new List<CategoryDTO>();
            pack.Categories.Add(await GetByType(Product_Category));
            pack.Categories.Add(await GetByType(Material_Category));
            pack.Categories.Add(await GetByType(ProductGroup_Category));
            return pack;
        }
        #endregion

        #region Post 新建场景信息
        /// <summary>
        /// 新建场景信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]CategoryCreateModel model)
        {
            //标准化类型字段
            model.Type = string.IsNullOrWhiteSpace(model.Type) ? string.Empty : model.Type.ToLower();

            var mapping = new Func<AssetCategory, Task<AssetCategory>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Icon = model.IconAssetId;
                entity.Type = model.Type;
                entity.ParentId = model.ParentId;
                entity.OrganizationId = CurrentAccountOrganizationId;
                return await Task.FromResult(entity);
            });

            var afterCreated = new Func<AssetCategory, Task>(async (entity) =>
            {
                //创建分类树
                var categoryTree = new AssetCategoryTree();
                categoryTree.ObjId = entity.Id;
                categoryTree.NodeType = entity.Type;
                categoryTree.OrganizationId = entity.OrganizationId;
                var refTree = await _CategoryTreeRepository.GetNodeByObjId(entity.ParentId);
                categoryTree.ParentId = refTree != null ? refTree.Id : string.Empty;
                await _CategoryTreeRepository.CreateAsync(categoryTree, CurrentAccountId);
            });

            #region 创建默认分类
            {
                var existRoot = await _Context.AssetCategories.AnyAsync(x => x.Type == model.Type && x.OrganizationId == CurrentAccountOrganizationId && x.IsRoot == true);
                if (!existRoot)
                {
                    //创建分类
                    var defaultCategory = new AssetCategory();
                    defaultCategory.Name = "Auto Create Category";
                    defaultCategory.Type = model.Type;
                    defaultCategory.IsRoot = true;
                    defaultCategory.OrganizationId = CurrentAccountOrganizationId;
                    await _Repository.CreateAsync(defaultCategory, CurrentAccountId);

                    //创建分类树
                    var categoryTree = new AssetCategoryTree();
                    categoryTree.ObjId = defaultCategory.Id;
                    categoryTree.NodeType = model.Type;
                    categoryTree.OrganizationId = defaultCategory.OrganizationId;
                    await _CategoryTreeRepository.CreateAsync(categoryTree, CurrentAccountId);
                    return await Get(defaultCategory.Id);
                }
            }
            #endregion

            return await _PostRequest(mapping, afterCreated);
        }
        #endregion

        #region Put 更新场景信息
        /// <summary>
        /// 更新场景信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]CategoryUpdateModel model)
        {
            var mapping = new Func<AssetCategory, Task<AssetCategory>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                if (!string.IsNullOrWhiteSpace(model.IconAssetId))
                    entity.Icon = model.IconAssetId;
                if (!string.IsNullOrWhiteSpace(model.ParentId))
                    entity.ParentId = model.ParentId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region Delete 删除数据信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            var afterDeleted = new Func<Task>(async () =>
            {
                var refNode = await _CategoryTreeRepository.GetNodeByObjId(id);
                await _CategoryTreeRepository.DeleteAsync(refNode.Id, CurrentAccountId);
            });
            return await _DeleteRequest(id, afterDeleted);
        }
        #endregion

        #region MoveUp 上移分类
        /// <summary> 
        /// 上移分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("MoveUp")]
        [HttpPut]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async Task<IActionResult> MoveUp(string id)
        {
            var cat = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (cat != null && cat.DisplayIndex >= 1)
            {
                var interChangeCat = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.ParentId == cat.ParentId && x.DisplayIndex == (cat.DisplayIndex - 1) && x.Type == cat.Type);
                if (interChangeCat != null)
                {
                    cat.DisplayIndex--;
                    interChangeCat.DisplayIndex++;
                    _Context.AssetCategories.Update(cat);
                    _Context.AssetCategories.Update(interChangeCat);
                    await _Context.SaveChangesAsync();
                    var res = await _GetParentCategoryAndSameLevelChildren(cat.ParentId);
                    return Ok(res);
                }

            }
            return BadRequest();
        }
        #endregion

        #region MoveDown 下移分类
        /// <summary>
        /// 下移分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("MoveDown")]
        [HttpPut]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async Task<IActionResult> MoveDown(string id)
        {
            var cat = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (cat != null)
            {
                var interChangeCat = await _Context.AssetCategories.FirstOrDefaultAsync(x => x.ParentId == cat.ParentId && x.DisplayIndex == (cat.DisplayIndex + 1) && x.Type == cat.Type);
                if (interChangeCat != null)
                {
                    cat.DisplayIndex++;
                    interChangeCat.DisplayIndex--;
                    _Context.AssetCategories.Update(cat);
                    _Context.AssetCategories.Update(interChangeCat);
                    await _Context.SaveChangesAsync();
                    var res = await _GetParentCategoryAndSameLevelChildren(cat.ParentId);
                    return Ok(res);
                }

            }
            return BadRequest();
        }
        #endregion
    }
}