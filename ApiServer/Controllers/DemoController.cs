using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Filters;
using ApiServer.Models;
using ApiServer.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Demo")]
    public class DemoController : Controller
    {
        [HttpGet]
        public IActionResult Post([FromQuery] PagingRequestModel model)
        {

            return Ok(1);
        }
    }

    //public class ValidateModelAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        if (!context.ModelState.IsValid)
    //        {
    //            var msg = context.ModelState.Select(x => x.Value);
    //            context.Result = new BadRequestObjectResult(context.ModelState);
    //        }
    //    }
    //}

    public class DemoEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

    }
}