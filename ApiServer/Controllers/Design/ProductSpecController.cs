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
    public class ProductSpecController : Controller
    {
        private readonly Repository<ProductSpec> repo;

        public ProductSpecController(Data.ApiDbContext context)
        {
            repo = new Repository<ProductSpec>(context);
        }

        [HttpGet("{id}")]
        [Produces(typeof(ProductSpec))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            repo.Context.Entry(res).Reference(d => d.StaticMesh).Load();
            return Ok(res);//return Forbid();
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
    }
}