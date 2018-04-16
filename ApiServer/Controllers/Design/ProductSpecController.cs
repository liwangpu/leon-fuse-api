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
            repo.Context.Entry(res).Collection(d => d.StaticMeshes).Load();
            if (res.StaticMeshes != null && res.StaticMeshes.Count > 0)
            {
                for (int idx = res.StaticMeshes.Count - 1; idx >= 0; idx--)
                {
                    var mesh = res.StaticMeshes[idx];
                    if (mesh != null)
                        mesh.FileAsset = await _ApiContext.Files.FindAsync(mesh.FileAssetId);
                }
            }
            if (!string.IsNullOrWhiteSpace(res.Icon))
            {
                var ass = await _ApiContext.Files.FindAsync(res.Icon);
                if (ass != null)
                    res.IconFileAsset = ass;
            }
            if (!string.IsNullOrWhiteSpace(res.Charlets))
            {
                var chartletIds = res.Charlets.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    var ass = await _ApiContext.Files.FindAsync(chartletIds[idx]);
                    if (ass != null)
                        res.CharletAsset.Add(ass);
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
                return Ok();
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
                var chartletIds = string.IsNullOrWhiteSpace(res.Charlets) ? new List<string>() : res.Charlets.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                chartletIds.Add(icon.AssetId);
                res.Charlets = string.Join(",", chartletIds);
                await _ApiContext.SaveChangesAsync();
                return Ok();
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
                var chartletIds = string.IsNullOrWhiteSpace(res.Charlets) ? new List<string>() : res.Charlets.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int idx = chartletIds.Count - 1; idx >= 0; idx--)
                {
                    if (chartletIds[idx] == icon.AssetId)
                        chartletIds.RemoveAt(idx);
                }
                res.Charlets = string.Join(",", chartletIds);
                await _ApiContext.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

    }
}