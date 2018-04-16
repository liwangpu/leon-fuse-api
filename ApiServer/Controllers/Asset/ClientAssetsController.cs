using ApiModel.Entities;
using ApiServer.Services;
using BambooCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    public class ClientAssetsController : Controller
    {
        private readonly Repository<ClientAsset> repo;

        public ClientAssetsController(Data.ApiDbContext context)
        {
            repo = new Repository<ClientAsset>(context);
        }

        [HttpGet]
        public async Task<PagedData> Get(string search, int page, int pageSize, string orderBy, bool desc)
        {
            PagingMan.CheckParam(ref search, ref page, ref pageSize);
            return await repo.GetAsync(AuthMan.GetAccountId(this), page, pageSize, orderBy, desc,d => d.Id.HaveSubStr(search) || d.Name.HaveSubStr(search) || d.Description.HaveSubStr(search));
        }

        [HttpGet("{id}")]
        [Produces(typeof(ClientAsset))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            return Ok(res);//return Forbid();
        }

        [Route("NewOne")]
        [HttpGet]
        public ClientAsset NewOne()
        {
            return EntityFactory<ClientAsset>.New();
        }

        [HttpPost]
        [Produces(typeof(ClientAsset))]
        public async Task<IActionResult> Post([FromBody]ClientAsset value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(ClientAsset))]
        public async Task<IActionResult> Put([FromBody]ClientAsset value)
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
