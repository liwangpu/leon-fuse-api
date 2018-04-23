using ApiModel.Entities;
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
    public class StaticMeshController : Controller
    {
        private readonly ApiDbContext _ApiContext;
        private readonly Repository<StaticMesh> repo;

        public StaticMeshController(ApiDbContext context)
        {
            repo = new Repository<StaticMesh>(context);
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
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Get(string id)
        {
            var res = await repo.GetAsync(AuthMan.GetAccountId(this), id);
            if (res == null)
                return NotFound();
            repo.Context.Entry(res).Collection(d => d.Materials).Load();
            if (res.Materials != null && res.Materials.Count > 0)
            {
                for (int idx = res.Materials.Count - 1; idx >= 0; idx--)
                {
                    var mate = res.Materials[idx];
                    mate.FileAsset = await _ApiContext.Files.FindAsync(mate.FileAssetId);
                }
            }
            return Ok(res.ToDictionary());//return Forbid();
        }


        [HttpPost]
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Post([FromBody]StaticMesh value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);
            if (string.IsNullOrEmpty(value.ProductSpecId) == false)
                value.ProductSpec = await repo.Context.Set<ProductSpec>().FindAsync(value.ProductSpecId);
            value = await repo.CreateAsync(AuthMan.GetAccountId(this), value);
            return CreatedAtAction("Get", value);
        }

        [HttpPut]
        [Produces(typeof(StaticMesh))]
        public async Task<IActionResult> Put([FromBody]StaticMesh value)
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