using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers.UIDesigner
{
    [Authorize]
    [Route("/[controller]")]
    public class BSModelController : Controller
    {
        [Route("List")]
        [HttpGet]
        public async Task<IActionResult> List(string modelName)
        {
            var model = new
            {
                Resource = modelName,
                ModelType = "List",
                Icon = "map",
                DisplayModel = new List<string>() { "List" },
                Fields = new List<string>() { "Id", "Name", "Description" }

            };
            return Ok(model);
        }
    }
}