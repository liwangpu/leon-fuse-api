using ApiModel.Entities;
using ApiServer.Data;
using ApiServer.Models;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly Repository<Department> repo;

        public DepartmentController(ApiDbContext context)
        {
            repo = new Repository<Department>(context);
        }

        [HttpGet("{id}")]
        [Produces(typeof(Department))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            await repo.Context.Entry(res).Collection(d => d.Members).LoadAsync();
            return Ok(res);//return Forbid();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DepartmentEditModel value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var dep = await repo.CreateAsync(AuthMan.GetAccountId(this), value.ToEntity());
            return CreatedAtAction("Get", dep);
        }

        [HttpPut]
        [Produces(typeof(Department))]
        public async Task<IActionResult> Put([FromBody]Department value)
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


        [Route("ByOrgan")]
        [HttpGet]
        [Produces(typeof(Department))]
        public async Task<IActionResult> GetByOrgan(string organId)
        {
            var res = await repo.GetDataSet(AuthMan.GetAccountId(this)).Where(x => x.OrganizationId == organId).ToListAsync();
            if (res == null)
                return NotFound();
            return Ok(res);
        }


    }
}