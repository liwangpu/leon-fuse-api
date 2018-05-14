﻿using ApiModel.Entities;
using ApiModel.Enums;
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
    /// <summary>
    /// 产品规格管理控制器
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    public class ProductSpecController : ListableController<ProductSpec, ProductSpecDTO>
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
                entity.ResourceType = (int)ResourceTypeEnum.Organizational;
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

        #region UploadAlbum 上传规格详细相册信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// <summary>
        /// 上传规格详细相册信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        [Route("UploadAlbum")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> UploadAlbum([FromBody]IconModel icon)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var albumIds = string.IsNullOrWhiteSpace(spec.Album) ? new List<string>() : spec.Album.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                albumIds.Add(icon.AssetId);
                spec.Album = string.Join(",", albumIds);
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
        [Route("DeleteAlbum")]
        [HttpPut]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(ValidationResultModel), 400)]
        public async Task<IActionResult> DeleteAlbum([FromBody]IconModel icon)
        {
            var mapping = new Func<ProductSpec, Task<ProductSpec>>(async (spec) =>
            {
                var albumIds = string.IsNullOrWhiteSpace(spec.Album) ? new List<string>() : spec.Album.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = albumIds.Count - 1; idx >= 0; idx--)
                {
                    if (albumIds[idx] == icon.AssetId)
                        albumIds.RemoveAt(idx);
                }
                spec.Album = string.Join(",", albumIds);
                return await Task.FromResult(spec);
            });
            return await _PutRequest(icon.ObjId, mapping);
        }
        #endregion

    }
}