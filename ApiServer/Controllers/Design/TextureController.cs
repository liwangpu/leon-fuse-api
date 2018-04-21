﻿using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class TextureController : Controller
    {
        private readonly ApiDbContext _ApiContext;
        private readonly Repository<Texture> repo;

        public TextureController(ApiDbContext context)
        {
            repo = new Repository<Texture>(context);
            _ApiContext = context;
        }

        [HttpGet]
        public async Task<PagedData> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
                d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }


        [HttpGet("{id}")]
        [Produces(typeof(Texture))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            return Ok(res.ToDictionary());//return Forbid();
        }


        [HttpPost]
        [Produces(typeof(Texture))]
        public async Task<IActionResult> Post([FromBody]Texture value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(Texture))]
        public async Task<IActionResult> Put([FromBody]Texture value)
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
    }
}