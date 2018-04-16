using ApiModel.Entities;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Design
{
    [Authorize]
    [Route("/[controller]")]
    public class ProductsController : Controller
    {
        private readonly Repository<Product> repo;

        public ProductsController(Data.ApiDbContext context)
        {
            repo = new Repository<Product>(context);
        }

        [HttpGet]
        public async Task<PagedData<Product>> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
               d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }


        [HttpGet("{id}")]
        [Produces(typeof(Product))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            repo.Context.Entry(res).Collection(d => d.Specifications).Load();
            return Ok(res.ToDictionary());//return Forbid();
        }

        [Route("NewOne")]
        [HttpGet]
        public Product NewOne()
        {
            return EntityFactory<Product>.New();
        }

        [HttpPost]
        [Produces(typeof(Product))]
        public async Task<IActionResult> Post([FromBody]Product value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(Product))]
        public async Task<IActionResult> Put([FromBody]Product value)
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
