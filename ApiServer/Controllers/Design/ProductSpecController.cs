using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.DotNet;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductSpecController : Controller
    {
        private readonly ApiDbContext _ApiContext;
        private readonly Repository<ProductSpec> repo;

        public ProductSpecController(ApiDbContext context)
        {
            repo = new Repository<ProductSpec>(context);
            _ApiContext = context;
        }

        [HttpGet("{id}")]
        [Produces(typeof(ProductSpec))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(res.Icon))
            {
                var ass = await _ApiContext.Files.FirstOrDefaultAsync(x => x.Id == res.Icon);
                if (ass != null)
                    res.IconFileAsset = ass;
            }
            if (!string.IsNullOrWhiteSpace(res.CharletIds))
            {
                var chartletIds = res.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    var ass = await _ApiContext.Files.FirstOrDefaultAsync(x => x.Id == chartletIds[idx]);
                    if (ass != null)
                        res.CharletAsset.Add(ass);
                }
            }
            if (!string.IsNullOrWhiteSpace(res.StaticMeshIds))
            {
                var meshIds = res.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = meshIds.Count - 1; idx >= 0; idx--)
                {
                    var kv = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(meshIds[idx]);
                    var refMesh = await _ApiContext.StaticMeshs.FirstOrDefaultAsync(x => x.Id == kv.Key);
                    if (refMesh != null)
                    {
                        //var tmp = await _ApiContext.Files.Where(x=>x.Id==refMesh.FileAssetId);
                        var tmp = await _ApiContext.Files.FirstOrDefaultAsync(x => x.Id == refMesh.FileAssetId);
                        if (tmp != null)
                            refMesh.FileAsset = tmp;
                    }

                    if (!string.IsNullOrWhiteSpace(kv.Value))
                    {
                        var matids = string.IsNullOrWhiteSpace(kv.Value) ? new List<string>() : kv.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (var item in matids)
                        {
                            var refMat = await _ApiContext.Materials.FirstOrDefaultAsync(x => x.Id == item);
                            if (refMat != null)
                            {
                                var tmp = await _ApiContext.Files.FirstOrDefaultAsync(x => x.Id == refMat.FileAssetId);
                                if (tmp != null)
                                {
                                    refMat.FileAsset = await _ApiContext.Files.FirstOrDefaultAsync(x => x.Id == refMat.FileAssetId);
                                    refMesh.Materials.Add(refMat);
                                }

                            }
                        }
                    }
                    res.StaticMeshAsset.Add(refMesh);
                }
            }
            return Ok(res.ToDictionary());//return Forbid();
        }


        [HttpPost]
        [Produces(typeof(ProductSpec))]
        public async Task<IActionResult> Post([FromBody]ProductSpec value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(ProductSpec))]
        public async Task<IActionResult> Put([FromBody]ProductSpec value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await repo.UpdateAsync(AuthMan.GetAccountId(this), value);
            if (res == null)
                return NotFound();
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool bOk = await repo.DeleteAsync(AuthMan.GetAccountId(this), id);
            if (bOk)
                return Ok();
            return NotFound();//return Forbid();
        }

        [Route("UploadStaticMesh")]
        [HttpPut]
        public async Task<IActionResult> UploadStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var spec = await _ApiContext.ProductSpec.FindAsync(mesh.ProductSpecId);
            if (spec != null)
            {
                //ids是一个KeyValuePair<string,string>的信息,key为static mesh id,value为mesh依赖的material ids(逗号分隔)
                var meshIds = string.IsNullOrWhiteSpace(spec.StaticMeshIds) ? new List<string>() : spec.StaticMeshIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                meshIds.Add(JsonConvert.SerializeObject(new KeyValuePair<string, string>(mesh.AssetId, "")));
                spec.StaticMeshIds = string.Join("|", meshIds);
                await _ApiContext.SaveChangesAsync();
                return Ok(mesh);
            }
            return NotFound(mesh);
        }

        [Route("DeleteStaticMesh")]
        [HttpPut]
        public async Task<IActionResult> DeleteStaticMesh([FromBody]SpecStaticMeshUploadModel mesh)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var spec = await _ApiContext.ProductSpec.FindAsync(mesh.ProductSpecId);
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
                await _ApiContext.SaveChangesAsync();
                return Ok(mesh);
            }
            return NotFound();
        }


        [Route("UploadMaterial")]
        [HttpPut]
        public async Task<IActionResult> UploadMaterial([FromBody]SpecMaterialUploadModel material)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var spec = await _ApiContext.ProductSpec.FindAsync(material.ProductSpecId);
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
                await _ApiContext.SaveChangesAsync();
                return Ok(material);
            }
            return NotFound();
        }

        [Route("DeleteMaterial")]
        [HttpPut]
        public async Task<IActionResult> DeleteMaterial([FromBody]SpecMaterialUploadModel material)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            var spec = await _ApiContext.ProductSpec.FindAsync(material.ProductSpecId);
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
                await _ApiContext.SaveChangesAsync();
                return Ok(material);
            }
            return NotFound();
        }

        [Route("ChangeICon")]
        [HttpPut]
        public async Task<IActionResult> ChangeICon([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await _ApiContext.ProductSpec.FindAsync(icon.ObjId);
            if (res != null)
            {
                res.Icon = icon.AssetId;
                await _ApiContext.SaveChangesAsync();
                return Ok(res);
            }
            return NotFound();
        }

        [Route("UploadChartlet")]
        [HttpPut]
        public async Task<IActionResult> UploadChartlet([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await _ApiContext.ProductSpec.FindAsync(icon.ObjId);
            if (res != null)
            {
                var chartletIds = string.IsNullOrWhiteSpace(res.CharletIds) ? new List<string>() : res.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                chartletIds.Add(icon.AssetId);
                res.CharletIds = string.Join(",", chartletIds);
                await _ApiContext.SaveChangesAsync();
                return Ok(res);
            }
            return NotFound();
        }

        [Route("DeleteChartlet")]
        [HttpPut]
        public async Task<IActionResult> DeleteChartlet([FromBody]IconModel icon)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await _ApiContext.ProductSpec.FindAsync(icon.ObjId);
            if (res != null)
            {
                var chartletIds = string.IsNullOrWhiteSpace(res.CharletIds) ? new List<string>() : res.CharletIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    if (chartletIds[idx] == icon.AssetId)
                        chartletIds.RemoveAt(idx);
                }
                res.CharletIds = string.Join(",", chartletIds);
                await _ApiContext.SaveChangesAsync();
                return Ok(icon);
            }
            return NotFound();
        }

    }
}