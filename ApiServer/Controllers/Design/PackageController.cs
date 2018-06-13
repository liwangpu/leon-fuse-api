﻿using ApiModel.Entities;
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
        { }
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
            return await _GetPagingRequest(model, null,null, literal);
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
                entity.Content = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(id, mapping);
        }
        #endregion
    }
}