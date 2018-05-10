﻿//using ApiModel.Entities;
//using ApiServer.Data;
//using ApiServer.Filters;
//using ApiServer.Models;
//using ApiServer.Services;
//using ApiServer.Stores;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ApiServer.Controllers.Design
//{
//    [Authorize]
//    [Route("/[controller]")]
//    public class ProductSpecController : Controller
//    {
//        private readonly ProductSpecStore _ProductSpecStore;

//        #region 构造函数
//        public ProductSpecController(ApiDbContext context)
//        {
//            _ProductSpecStore = new ProductSpecStore(context);
//        }
//        #endregion

//        #region Put 编辑产品规格信息
//        /// <summary>
//        /// 编辑产品规格信息
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        [HttpPut]
//        [ValidateModel]
//        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
//        [ProducesResponseType(typeof(ValidationResultModel), 400)]
//        public async Task<IActionResult> Put([FromBody]ProductSpecEditModel value)
//        {
//            var exist = await _ProductSpecStore.Exist(value.Id);
//            if (!exist)
//                return NotFound();

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(value.Id);
//            spec.Name = value.Name;
//            spec.Description = value.Description;
//            spec.ModifiedTime = DateTime.Now;
//            spec.Price = value.Price;
//            await _ProductSpecStore.CanUpdate(accid, spec, ModelState);
//            if (!ModelState.IsValid)
//                return new ValidationFailedResult(ModelState);

//            var dto = await _ProductSpecStore.UpdateAsync(accid, spec);
//            return Ok(dto);
//        }
//        #endregion

//        #region Delete 删除产品规格信息
//        /// <summary>
//        /// 删除产品规格信息
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var exist = await _ProductSpecStore.Exist(id);
//            if (!exist)
//                return NotFound();

//            var accid = AuthMan.GetAccountId(this);
//            var canDelete = await _ProductSpecStore.CanDelete(accid, id);
//            if (!canDelete)
//                return Forbid();
//            await _ProductSpecStore.DeleteAsync(accid, id);
//            return Ok();
//        }
//        #endregion

//        #region UploadStaticMesh 上传模型文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
//        /// <summary>
//        /// 上传模型文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
//        /// </summary>
//        /// <param name="mesh"></param>
//        /// <returns></returns>
//        [Route("UploadStaticMesh")]
//        [HttpPut]
//        [ProducesResponseType(typeof(SpecStaticMeshUploadModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public async Task<IActionResult> UploadStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(mesh.ProductSpecId);
//            if (spec != null)
//            {
//                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
//                var exist = map.Items.Where(x => x.StaticMeshId == mesh.AssetId).Count() > 0;
//                if (!exist)
//                {
//                    var item = new SpecMeshMapItem();
//                    item.StaticMeshId = mesh.AssetId;
//                    map.Items.Add(item);
//                }
//                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
//                await _ProductSpecStore._SaveOrUpdateAsync(spec);
//                return Ok(mesh);
//            }
//            return NotFound(mesh);
//        }
//        #endregion

//        #region DeleteStaticMesh 删除模型文件信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// <summary>
//        /// 删除模型文件信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// </summary>
//        /// <param name="mesh"></param>
//        /// <returns></returns>
//        [Route("DeleteStaticMesh")]
//        [HttpPut]
//        [ProducesResponseType(typeof(SpecStaticMeshUploadModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public async Task<IActionResult> DeleteStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(mesh.ProductSpecId);
//            if (spec != null)
//            {
//                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
//                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
//                {
//                    if (map.Items[idx].StaticMeshId == mesh.AssetId)
//                        map.Items.RemoveAt(idx);
//                }
//                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
//                await _ProductSpecStore._SaveOrUpdateAsync(spec);
//                return Ok(mesh);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region UploadMaterial 上传材质文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)不用异步,因为多文件上传异步出先同时进行造成信息丢失
//        /// <summary>
//        /// 上传材质文件信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
//        /// </summary>
//        /// <param name="material"></param>
//        /// <returns></returns>
//        [Route("UploadMaterial")]
//        [HttpPut]
//        [ProducesResponseType(typeof(SpecMaterialUploadModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public IActionResult UploadMaterial([FromBody]SpecMaterialUploadModel material)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = _ProductSpecStore.GetById(material.ProductSpecId);
//            if (spec != null)
//            {
//                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
//                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
//                {
//                    if (map.Items[idx].StaticMeshId == material.StaticMeshId)
//                    {
//                        var metids = map.Items[idx].MaterialIds == null ? new List<string>() : map.Items[idx].MaterialIds;
//                        metids.Add(material.AssetId);
//                        map.Items[idx].MaterialIds = metids;
//                    }
//                }
//                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
//                _ProductSpecStore._SaveOrUpdate(spec);
//                return Ok(material);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region DeleteMaterial 删除材质文件信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// <summary>
//        /// 删除材质文件信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// </summary>
//        /// <param name="material"></param>
//        /// <returns></returns>
//        [Route("DeleteMaterial")]
//        [HttpPut]
//        [ProducesResponseType(typeof(SpecMaterialUploadModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public async Task<IActionResult> DeleteMaterial([FromBody]SpecMaterialUploadModel material)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(material.ProductSpecId);
//            if (spec != null)
//            {
//                var map = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new SpecMeshMap() : JsonConvert.DeserializeObject<SpecMeshMap>(spec.StaticMeshIds);
//                for (int idx = map.Items.Count - 1; idx >= 0; idx--)
//                {
//                    if (map.Items[idx].StaticMeshId == material.StaticMeshId)
//                    {
//                        var metids = map.Items[idx].MaterialIds;
//                        for (int ndx = metids.Count - 1; ndx >= 0; ndx--)
//                        {
//                            if (metids[ndx] == material.AssetId)
//                                metids.RemoveAt(ndx);
//                        }
//                        map.Items[idx].MaterialIds = metids;
//                    }
//                }
//                spec.StaticMeshIds = JsonConvert.SerializeObject(map);
//                await _ProductSpecStore._SaveOrUpdateAsync(spec);
//                return Ok(material);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region ChangeICon 更新图标信息
//        /// <summary>
//        /// 更新图标信息
//        /// </summary>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        [Route("ChangeICon")]
//        [HttpPut]
//        [ProducesResponseType(typeof(IconModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(icon.ObjId);
//            if (spec != null)
//            {
//                spec.Icon = icon.AssetId;
//                await _ProductSpecStore._SaveOrUpdateAsync(spec);
//                return Ok(spec);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region UploadChartlet 上传规格详细图片信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
//        /// <summary>
//        /// 上传规格详细图片信息(非真实上传文件,文件已经提交到File,此处只是添加文件id到规格相关字段信息)
//        /// </summary>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        [Route("UploadChartlet")]
//        [HttpPut]
//        [ProducesResponseType(typeof(IconModel), 200)]
//        [ProducesResponseType(typeof(List<string>), 404)]
//        public IActionResult UploadChartlet([FromBody]IconModel icon)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec =  _ProductSpecStore.GetById(icon.ObjId);
//            if (spec != null)
//            {
//                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
//                chartletIds.Add(icon.AssetId);
//                spec.CharletIds = string.Join(",", chartletIds);
//                 _ProductSpecStore._SaveOrUpdate(spec);
//                return Ok(spec);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region DeleteChartlet 删除规格详细图片信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// <summary>
//        /// 删除规格详细图片信息(文件已经提交到File,删除不会对文件进行真实删除)
//        /// </summary>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        [Route("DeleteChartlet")]
//        [HttpPut]
//        public async Task<IActionResult> DeleteChartlet([FromBody]IconModel icon)
//        {
//            if (ModelState.IsValid == false)
//                return BadRequest(ModelState);

//            var accid = AuthMan.GetAccountId(this);
//            var spec = await _ProductSpecStore.GetByIdAsync(icon.ObjId);
//            if (spec != null)
//            {
//                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
//                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
//                {
//                    if (chartletIds[idx] == icon.AssetId)
//                        chartletIds.RemoveAt(idx);
//                }
//                spec.CharletIds = string.Join(",", chartletIds);
//                await _ProductSpecStore._SaveOrUpdateAsync(spec);
//                return Ok(icon);
//            }
//            return NotFound();
//        }
//        #endregion

//        #region NewOne Post,Put示例数据
//        /// <summary>
//        /// Post,Put示例数据
//        /// </summary>
//        /// <returns></returns>
//        [Route("NewOne")]
//        [HttpGet]
//        [ProducesResponseType(typeof(ProductSpecCreateModel), 200)]
//        public IActionResult NewOne()
//        {
//            return Ok(new ProductSpecCreateModel());
//        }
//        #endregion

//    }
//}