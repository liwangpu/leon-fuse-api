using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using ApiModel.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers.Global
{
    [Authorize]
    [Route("/[controller]")]
    public class SettingsController : Controller
    {
        Data.ApiDbContext context;

        public SettingsController(Data.ApiDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<SettingsItem> Get()
        {
            return context.Settings;
        }

        [HttpGet("{key}")]
        public string Get(string key)
        {
            return Services.SiteConfig.Instance.GetItem(key, context);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SettingsItem value)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            await Services.SiteConfig.Instance.SetItem(value.Key, value.Value, context);
            return Ok();
        }

        [HttpPut("{key}")]
        public async Task Put(string key, [FromBody]string value)
        {
            if (value == null)
                value = "";
            await Services.SiteConfig.Instance.SetItem(key, value, context);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            bool bOk =await Services.SiteConfig.Instance.DeleteItem(key, context);
            if (bOk)
                return Ok();
            return NotFound();
        }

        [Route("Reload")]
        [HttpPost]
        public void Reload()
        {
            Services.SiteConfig.Instance.ReloadSettingsFromDb(context);
        }
    }
}
