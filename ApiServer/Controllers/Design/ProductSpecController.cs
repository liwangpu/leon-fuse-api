using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using ApiServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductSpecController : Controller
    {
        private readonly ProductSpecStore _ProductSpecStore;

        #region 构造函数
        public ProductSpecController(ApiDbContext context)
        {
            _ProductSpecStore = new ProductSpecStore(context);
        }
        #endregion

        #region Get 根据id获取产品规格信息
        /// <summary>
        /// 根据id获取产品规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductSpecDTO), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Get(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductSpecStore.CanRead(accid, id);
            if (msg.Count > 0)
                return NotFound(msg);
            var data = await _ProductSpecStore.GetByIdAsync(accid, id);
            return Ok(data);
        }
        #endregion

        #region Post 新建产品规格信息
        /// <summary>
        /// 新建产品规格信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductSpec), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post([FromBody]ProductSpecEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var spec = new ProductSpec();
            spec.Name = value.Name;
            spec.Description = value.Description;
            spec.ProductId = value.ProductId;
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductSpecStore.CanCreate(accid, spec);
            if (msg.Count > 0)
                return BadRequest(msg);
            await _ProductSpecStore.SaveOrUpdateAsync(accid, spec);
            return Ok(spec);
        }
        #endregion

        #region Put 编辑产品规格信息
        /// <summary>
        /// 编辑产品规格信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ProductSpec), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Put([FromBody]ProductSpecEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(value.Id);
            if (spec == null)
                return BadRequest(new List<string>() { ValidityMessage.V_NotDataOrPermissionMsg });
            spec.Name = value.Name;
            spec.Description = value.Description;
            spec.CategoryId = value.CategoryId;
            spec.ModifiedTime = DateTime.Now;
            var msg = await _ProductSpecStore.CanUpdate(accid, spec);
            if (msg.Count > 0)
                return BadRequest(msg);
            await _ProductSpecStore.SaveOrUpdateAsync(accid, spec);
            return Ok(spec);
        }
        #endregion

        #region Delete 删除产品规格信息
        /// <summary>
        /// 删除产品规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Nullable), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> Delete(string id)
        {
            var accid = AuthMan.GetAccountId(this);
            var msg = await _ProductSpecStore.CanDelete(accid, id);
            if (msg.Count > 0)
                return NotFound(msg);
            await _ProductSpecStore.DeleteAsync(accid, id);
            return Ok();
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
        [ProducesResponseType(typeof(SpecStaticMeshUploadModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> UploadStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(mesh.ProductSpecId);
            if (spec != null)
            {
                //ids是一个KeyValuePair<string,string>的信息,key为static mesh id,value为mesh依赖的material ids(逗号分隔)
                var meshIds = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new List<string>() : spec.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                meshIds.Add(JsonConvert.SerializeObject(new KeyValuePair<string, string>(mesh.AssetId, "")));
                spec.StaticMeshIds = string.Join("|", meshIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(mesh);
            }
            return NotFound(mesh);
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
        [ProducesResponseType(typeof(SpecStaticMeshUploadModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> DeleteStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(mesh.ProductSpecId);
            if (spec != null)
            {
                var meshIds = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new List<string>() : spec.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = meshIds.Count - 1; idx >= 0; idx--)
                {
                    var kv = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(meshIds[idx]);
                    if (kv.Key == mesh.AssetId)
                        meshIds.RemoveAt(idx);
                }
                spec.StaticMeshIds = string.Join("|", meshIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(mesh);
            }
            return NotFound();
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
        [ProducesResponseType(typeof(SpecMaterialUploadModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> UploadMaterial([FromBody]SpecMaterialUploadModel material)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(material.ProductSpecId);
            if (spec != null)
            {
                var meshIds = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new List<string>() : spec.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = meshIds.Count - 1; idx >= 0; idx--)
                {
                    var kv = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(meshIds[idx]);
                    if (kv.Key == material.StaticMeshId)
                    {
                        var metids = string.IsNullOrWhiteSpace(kv.Value) ? new List<string>() : kv.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        metids.Add(material.AssetId);
                        meshIds[idx] = JsonConvert.SerializeObject(new KeyValuePair<string, string>(kv.Key, string.Join(",", metids)));
                    }
                }
                spec.StaticMeshIds = string.Join("|", meshIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(material);
            }
            return NotFound();
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
        [ProducesResponseType(typeof(SpecMaterialUploadModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> DeleteMaterial([FromBody]SpecMaterialUploadModel material)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(material.ProductSpecId);
            if (spec != null)
            {
                var meshIds = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new List<string>() : spec.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = meshIds.Count - 1; idx >= 0; idx--)
                {
                    var kv = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(meshIds[idx]);
                    if (kv.Key == material.StaticMeshId)
                    {
                        var metids = string.IsNullOrWhiteSpace(kv.Value) ? new List<string>() : kv.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        for (int ndx = metids.Count - 1; ndx >= 0; ndx--)
                        {
                            if (metids[ndx] == material.AssetId)
                                metids.RemoveAt(ndx);
                        }
                        meshIds[idx] = JsonConvert.SerializeObject(new KeyValuePair<string, string>(kv.Key, string.Join(",", metids)));
                    }
                }
                spec.StaticMeshIds = string.Join("|", meshIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(material);
            }
            return NotFound();
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
        [ProducesResponseType(typeof(IconModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(icon.ObjId);
            if (spec != null)
            {
                spec.Icon = icon.AssetId;
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(spec);
            }
            return NotFound();
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
        [ProducesResponseType(typeof(IconModel), 200)]
        [ProducesResponseType(typeof(List<string>), 404)]
        public async Task<IActionResult> UploadChartlet([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(icon.ObjId);
            if (spec != null)
            {
                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                chartletIds.Add(icon.AssetId);
                spec.CharletIds = string.Join(",", chartletIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(spec);
            }
            return NotFound();
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
        public async Task<IActionResult> DeleteChartlet([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var accid = AuthMan.GetAccountId(this);
            var spec = await _ProductSpecStore._GetByIdAsync(icon.ObjId);
            if (spec != null)
            {
                var chartletIds = string.IsNullOrWhiteSpace(spec.CharletIds) ? new List<string>() : spec.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    if (chartletIds[idx] == icon.AssetId)
                        chartletIds.RemoveAt(idx);
                }
                spec.CharletIds = string.Join(",", chartletIds);
                await _ProductSpecStore._SaveOrUpdateAsync(spec);
                return Ok(icon);
            }
            return NotFound();
        }
        #endregion

    }
}