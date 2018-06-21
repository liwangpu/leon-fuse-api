using ApiModel.Entities;
using ApiModel.Enums;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 套餐管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class PackageController : ListableController<Package, PackageDTO>
    {
        #region 构造函数
        public PackageController(ApiDbContext context)
        : base(new PackageStore(context))
        {
        }
        #endregion

        #region Get 根据分页查询信息获取套餐概要信息
        /// <summary>
        /// 根据分页查询信息获取套餐概要信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<PackageDTO>), 200)]
        public async Task<IActionResult> Get([FromQuery] PagingRequestModel model)
        {
            var literal = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.Content = null;
                return await Task.FromResult(entity);
            });
            return await _GetPagingRequest(model, null, null, literal);
        }
        #endregion

        #region Get 根据id获取套餐信息
        /// <summary>
        /// 根据id获取套餐信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建套餐信息
        /// <summary>
        /// 新建套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]PackageCreateModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Content = model.Content;
                entity.Icon = model.IconAssetId;
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 更新套餐信息
        /// <summary>
        /// 更新套餐信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]PackageEditModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Content = model.Content;
                entity.Icon = model.IconAssetId;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region EditAreaType 编辑套餐区域信息
        /// <summary>
        /// 编辑套餐区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("EditAreaType")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> EditAreaType([FromBody]PackageAreaTypeEditModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                var bExist = false;
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();

                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.Id)
                    {
                        curItem.AreaAlias = model.AreaAlias;
                        curItem.AreaTypeId = model.AreaTypeId;
                        bExist = true;
                        break;
                    }
                }

                if (!bExist)
                {
                    areas.Add(new PackageArea() { AreaAlias = model.AreaAlias, AreaTypeId = model.AreaTypeId, Id = GuidGen.NewGUID() });
                }
                entity.ContentIns.Areas = areas;
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteAreaType 删除套餐区域信息
        /// <summary>
        /// 删除套餐区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteAreaType")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteAreaType([FromBody]PackageAreaTypeDeleteModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();

                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.Id)
                    {
                        areas.RemoveAt(idx);
                        break;
                    }
                }
                entity.ContentIns.Areas = areas;
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region AddProductGroup 添加套餐区域产品组信息
        /// <summary>
        /// 添加套餐区域产品组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddProductGroup")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> AddProductGroup([FromBody]PackageProductGroupCreateModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var groupDic = curItem.GroupsMap != null ? curItem.GroupsMap : new Dictionary<string, string>();
                        groupDic[model.Serie] = model.ProductGroupId;
                        curItem.GroupsMap = groupDic;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteProductGroup 删除套餐区域产品组信息
        /// <summary>
        /// 删除套餐区域产品组信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteProductGroup")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteProductGroup([FromBody]PackageProductGroupDeleteModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var groupDic = curItem.GroupsMap != null ? curItem.GroupsMap : new Dictionary<string, string>();
                        for (int nxd = 0; nxd >= 0; nxd--)
                        {
                            var curKv = groupDic.ElementAt(nxd);
                            if (curKv.Value == model.ProductGroupId)
                                groupDic.Remove(curKv.Key);
                        }
                        curItem.GroupsMap = groupDic;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region AddCategoryProduct 添加套餐区域分类产品信息
        /// <summary>
        /// 添加套餐区域分类产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddCategoryProduct")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> AddCategoryProduct([FromBody]PackageCategoryProductCreateModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var cateogoryDic = curItem.ProductCategoryMap != null ? curItem.ProductCategoryMap : new Dictionary<string, string>();
                        cateogoryDic[model.ProductCategoryId] = model.ProductId;
                        curItem.ProductCategoryMap = cateogoryDic;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteCategoryProduct 删除套餐区域分类产品信息
        /// <summary>
        /// 删除套餐区域分类产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteCategoryProduct")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteCategoryProduct([FromBody]PackageCategoryProductDeleteModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var cateogoryDic = curItem.ProductCategoryMap != null ? curItem.ProductCategoryMap : new Dictionary<string, string>();
                        for (int nxd = 0; nxd >= 0; nxd--)
                        {
                            var curKv = cateogoryDic.ElementAt(nxd);
                            if (curKv.Value == model.ProductId)
                                cateogoryDic.Remove(curKv.Key);
                        }
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region EditMaterial 新增/编辑套餐材质信息
        /// <summary>
        /// 新增/编辑套餐材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("EditMaterial")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> EditMaterial([FromBody]PackageMaterialCreateModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var materialDic = curItem.Materials != null ? curItem.Materials : new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(model.LastActorName) && materialDic[model.LastActorName] != null)
                            materialDic.Remove(model.LastActorName);
                        materialDic[!string.IsNullOrWhiteSpace(model.ActorName) ? model.ActorName : "待定"] = model.MaterialId;
                        curItem.Materials = materialDic;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteMaterial 删除套餐材质信息
        /// <summary>
        /// 删除套餐材质信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteMaterial")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteMaterial([FromBody]PackageMaterialDeleteModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int idx = areas.Count - 1; idx >= 0; idx--)
                {
                    var curItem = areas[idx];
                    if (curItem.Id == model.AreaId)
                    {
                        var materialDic = curItem.Materials != null ? curItem.Materials : new Dictionary<string, string>();
                        for (int nxd = 0; nxd >= 0; nxd--)
                        {
                            var curKv = materialDic.ElementAt(nxd);
                            if (curKv.Key == model.LastActorName && curKv.Value == model.MaterialId)
                            {
                                materialDic.Remove(curKv.Key);
                                break;
                            }
                        }
                        curItem.Materials = materialDic;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region AddReplaceGroup 添加套餐替换组
        /// <summary>
        /// 添加套餐替换组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddReplaceGroup")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> AddReplaceGroup([FromBody]PackageReplaceGroupCreateModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int k = areas.Count - 1; k >= 0; k--)
                {
                    var curItem = areas[k];
                    if (curItem.Id == model.AreaId)
                    {
                        var replaceList = curItem.ReplaceGroups != null ? curItem.ReplaceGroups : new List<PackageProductSet>();
                        var productIdArr = model.ProductIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        foreach (var inputProductId in productIdArr)
                        {
                            var bExistCateogry = false;

                            #region 遍历所有替换组,往已经存在的替换组里面添加产品信息
                            for (int ndx = 0, len = replaceList.Count; ndx < len; ndx++)
                            {
                                var curReplaceGroup = replaceList[ndx];
                                if (curReplaceGroup.Products == null)
                                    curReplaceGroup.Products = new List<string>();
                                var aaa = curReplaceGroup.DefaultId;
                                //var defaultProduct = await _DbContext.Products.FindAsync(curReplaceGroup.DefaultId);
                                var defaultProduct = await _Store.DbContext.Products.FirstOrDefaultAsync(x => x.Id == curReplaceGroup.DefaultId);
                                var curProduct = await _Store.DbContext.Products.FindAsync(inputProductId);
                                if (defaultProduct != null)
                                {
                                    if (defaultProduct.CategoryId == curProduct.CategoryId)
                                    {
                                        bExistCateogry = true;
                                        if (defaultProduct.Id != curProduct.Id && !curReplaceGroup.Products.Exists(x => x == inputProductId))
                                            curReplaceGroup.Products.Add(inputProductId);
                                    }
                                }
                            }
                            #endregion

                            #region 替换组不存在,新建一个替换组
                            if (!bExistCateogry)
                            {
                                var nset = new PackageProductSet();
                                nset.DefaultId = inputProductId;
                                nset.Products = new List<string>() { inputProductId };
                                replaceList.Add(nset);
                            }
                            #endregion
                        }
                        curItem.ReplaceGroups = replaceList;
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteReplaceGroup 删除套餐替换组
        /// <summary>
        /// 删除套餐替换组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteReplaceGroup")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteReplaceGroup([FromBody]PackageReplaceGroupDeleteModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int k = areas.Count - 1; k >= 0; k--)
                {
                    var curItem = areas[k];
                    if (curItem.Id == model.AreaId)
                    {
                        var replaceList = curItem.ReplaceGroups != null ? curItem.ReplaceGroups : new List<PackageProductSet>();
                        for (int idx = replaceList.Count - 1; idx >= 0; idx--)
                        {
                            var curGroup = replaceList[idx];
                            if (curGroup.DefaultId == model.ProductId)
                            {
                                replaceList.RemoveAt(idx);
                                break;
                            }
                        }
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region SetDefaultReplaceGroup 设置替换组默认项
        /// <summary>
        /// 设置替换组默认项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SetDefaultReplaceGroup")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> SetDefaultReplaceGroup([FromBody] PackageReplaceGroupSetDefaultModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int k = areas.Count - 1; k >= 0; k--)
                {
                    var curItem = areas[k];
                    if (curItem.Id == model.AreaId)
                    {
                        var replaceList = curItem.ReplaceGroups != null ? curItem.ReplaceGroups : new List<PackageProductSet>();
                        for (int idx = replaceList.Count - 1; idx >= 0; idx--)
                        {
                            var curGroup = replaceList[idx];
                            if (curGroup.DefaultId == model.DefaultId)
                            {
                                curGroup.DefaultId = model.ProductId;
                                break;
                            }
                        }
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region DeleteReplaceGroupDetailItem 删除替换组产品项
        /// <summary>
        /// 删除替换组产品项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteReplaceGroupDetailItem")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(PackageDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteReplaceGroupDetailItem([FromBody] PackageReplaceGroupSetDefaultModel model)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.ContentIns = !string.IsNullOrWhiteSpace(entity.Content) ? JsonConvert.DeserializeObject<PackageContent>(entity.Content) : new PackageContent();
                var areas = entity.ContentIns != null && entity.ContentIns.Areas != null && entity.ContentIns.Areas.Count > 0 ? entity.ContentIns.Areas : new List<PackageArea>();
                for (int k = areas.Count - 1; k >= 0; k--)
                {
                    var curItem = areas[k];
                    if (curItem.Id == model.AreaId)
                    {
                        var replaceList = curItem.ReplaceGroups != null ? curItem.ReplaceGroups : new List<PackageProductSet>();
                        for (int idx = replaceList.Count - 1; idx >= 0; idx--)
                        {
                            var curGroup = replaceList[idx];
                            if (curGroup.DefaultId == model.DefaultId)
                            {
                                //删除的是默认项,找不是默认项的第一个设为默认
                                if (curGroup.DefaultId == model.ProductId)
                                {
                                    //改组只有一个默认项,删除改组
                                    if (curGroup.Products.Count == 1)
                                    {
                                        replaceList.RemoveAt(idx);
                                        break;
                                    }
                                }
                                //清除组里面要删除的项
                                for (int dix = curGroup.Products.Count - 1; dix >= 0; dix--)
                                {
                                    if (curGroup.Products[dix] == model.ProductId)
                                        curGroup.Products.RemoveAt(dix);
                                }
                                curGroup.DefaultId = curGroup.Products[0];
                                break;
                            }
                        }
                        break;
                    }
                }
                entity.Content = JsonConvert.SerializeObject(entity.ContentIns);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.PackageId, mapping);
        }
        #endregion

        #region ChangeContent 更新套餐详情信息
        /// <summary>
        /// 更新套餐详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("ChangeContent")]
        [HttpPost]
        public async Task<IActionResult> ChangeContent(string id, [FromBody]OrderContent content)
        {
            var mapping = new Func<Package, Task<Package>>(async (entity) =>
            {
                entity.Content = JsonConvert.SerializeObject(content);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(id, mapping);
        }
        #endregion
    }
}