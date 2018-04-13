using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ApiModel;
using BambooCommon;
using BambooCore;
using ApiServer.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class LayoutController : Controller
    {
        private readonly Repository<Layout> repo;

        public LayoutController(Data.ApiDbContext context)
        {
            repo = new Repository<Layout>(context);
        }

        [HttpGet]
        public async Task<PagedData<Layout>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
                d => d.Id.HaveSubStr(search) || d.Name.HaveSubStr(search) || d.Description.HaveSubStr(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(Layout))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            return Ok(res);//return Forbid();
        }

        [Route("NewOne")]
        [HttpGet]
        public Layout NewOne()
        {
            return EntityFactory<Layout>.New();
        }

        [HttpPost]
        [Produces(typeof(Layout))]
        public async Task<IActionResult> Post([FromBody]Layout value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(Layout))]
        public async Task<IActionResult> Put([FromBody]Layout value)
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

        [Route("UpdateData")]
        [HttpPost]
        public async Task<IActionResult> UpdateData(string id, [FromBody]string data)
        {
            if (data == null)
                data = "";
            string accid = AuthMan.GetAccountId(this);
            var ok = await repo.CanUpdateAsync(accid, id);
            if (ok == false)
                return Forbid();

            var obj = await repo.GetAsync(accid, id);
            if (obj == null)
                return Forbid();
            obj.Data = data;
            await repo.SaveChangesAsync();

            return Ok();
        }
    }
}
