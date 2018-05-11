﻿using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductSpecController : ListableController<ProductSpec>
    {
        #region 构造函数
        public ProductSpecController(ApiDbContext context)
        : base(new ProductSpecStore(context))
        { }
        #endregion

        #region Get 根据id获取产品规格信息
        /// <summary>
        /// 根据id获取产品规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        public async Task<IActionResult> Get(string id)
        {
            return await _GetByIdRequest(id);
        }
        #endregion

        #region Post 新建产品规格信息
        /// <summary>
        /// 新建产品规格信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Post([FromBody]ProductSpecCreateModel model)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.ProductId = model.ProductId;
                entity.Price = model.Price;
                return await Task.FromResult(entity);
            });
            return await _PostRequest(mapping);
        }
        #endregion

        #region Put 编辑产品规格信息
        /// <summary>
        /// 编辑产品规格信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> Put([FromBody]ProductSpecEditModel model)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (entity) =>
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                return await Task.FromResult(entity);
            });
            return await _PutRequest(model.Id, mapping);
        }
        #endregion

        #region UploadStaticMesh 上传模型文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// <summary>
        /// 上传模型文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        [Route("UploadStaticMesh")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UploadStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (entity) =>
            {
                var map = string.IsNullOrWhiteSpace(entity.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(entity.StaticMeshIds);
                var exist = map.Items.Where(x => x.StaticMeshId == mesh.AssetId).Count() > 0;
                if (!exist)
                {
                    var item = new SpecMeshMapItem();
                    item.StaticMeshId = mesh.AssetId;
                    map.Items.Add(item);
                }
                entity.StaticMeshIds = JsonConvert.SerializeObject(map);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(mesh.ProductSpecId, mapping);
        }
        #endregion

        #region DeleteStaticMesh 删除模型文件信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// <summary>
        /// 删除模型文件信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        [Route("DeleteStaticMesh")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (entity) =>
            {
                var map = string.IsNullOrWhiteSpace(entity.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(entity.StaticMeshIds);
                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                {
                    if (map.Items[idx].StaticMeshId == mesh.AssetId)
                        map.Items.RemoveAt(idx);
                }
                entity.StaticMeshIds = JsonConvert.SerializeObject(map);
                return await Task.FromResult(entity);
            });
            return await _PutRequest(mesh.ProductSpecId, mapping);
        }
        #endregion

        #region UploadMaterial 上传材质文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// <summary>
        /// 上传材质文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        [Route("UploadMaterial")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UploadMaterial([FromBody]SpecMaterialUploadModel material)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                {
                    if (map.Items[idx].StaticMeshId == material.StaticMeshId)
                    {
                        var metids = map.Items[idx].MaterialIds == null ? new List<string>() : map.Items[idx].MaterialIds;
                        metids.Add(material.AssetId);
                        map.Items[idx].MaterialIds = metids;
                    }
                }
                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
                return await Task.FromResult(spec);
            });
            return await _PutRequest(material.ProductSpecId, mapping);
        }
        #endregion

        #region DeleteMaterial 删除材质文件信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// <summary>
        /// 删除材质文件信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        [Route("DeleteMaterial")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteMaterial([FromBody]SpecMaterialUploadModel material)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
                {
                    if (map.Items[idx].StaticMeshId == material.StaticMeshId)
                    {
                        var metids = map.Items[idx].MaterialIds;
                        for (int ndx = metids.Count - 1; ndx >= 0; ndx--)
                        {
                            if (metids[ndx] == material.AssetId)
                                metids.RemoveAt(ndx);
                        }
                        map.Items[idx].MaterialIds = metids;
                    }
                }
                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
                return await Task.FromResult(spec);
            });
            return await _PutRequest(material.ProductSpecId, mapping);
        }
        #endregion

        #region ChangeICon 更新图标信息
        /// <summary>
        /// 更新图标信息
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("ChangeICon")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                spec.Icon = icon.AssetId;
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

        #region UploadChartlet 上传规格详细图片信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// <summary>
        /// 上传规格详细图片信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("UploadChartlet")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UploadChartlet([FromBody]IconModel icon)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                chartletIds.Add(icon.AssetId);
                spec.CharletIds = string.Join(",", chartletIds);
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

        #region DeleteChartlet 删除规格详细图片信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// <summary>
        /// 删除规格详细图片信息(文件已经提交到File,删除不会对文件进行真实删除)
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("DeleteChartlet")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteChartlet([FromBody]IconModel icon)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    if (chartletIds[idx] == icon.AssetId)
                        chartletIds.RemoveAt(idx);
                }
                spec.CharletIds = string.Join(",", chartletIds);
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

    }
}