using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class OrganController : Controller
    {
        private readonly Repository<Organization> repo;

        public OrganController(ApiDbContext context)
        {
            repo = new Repository<Organization>(context);
        }

        [HttpGet]
        public async Task<PagedData> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,
               d => d.Id.Contains(search) || d.Name.Contains(search) || d.Description.Contains(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(Organization))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            await repo.Context.Entry(res).Reference(d => d.Owner).LoadAsync();

            return Ok(res.ToDictionary());//return Forbid();
        }

        [HttpPost]
        [Produces(typeof(Organization))]
        public async Task<IActionResult> Post([FromBody]Organization value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody]Organization value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var res = await repo.UpdateAsync(AuthMan.GetAccountId(this), value);
            if (res == null)
                return NotFound();
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
