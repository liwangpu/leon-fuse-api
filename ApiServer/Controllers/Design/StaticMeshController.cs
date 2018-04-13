using ApiModel;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class StaticMeshController : Controller
    {
        private readonly Repository<StaticMesh> repo;

        public StaticMeshController(Data.ApiDbContext context)
        {
            repo = new Repository<StaticMesh>(context);
        }

        [HttpGet]
        public async Task<PagedData<StaticMesh>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
                d => d.Id.HaveSubStr(search) || d.Name.HaveSubStr(search) || d.Description.HaveSubStr(search));
        }


        [HttpGet("{id}")]
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            return Ok(res);//return Forbid();
        }


        [HttpPost]
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Post([FromBody]StaticMesh value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Put([FromBody]StaticMesh value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res =await repo.UpdateAsync(AuthMan.GetAccountId(this), value);
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