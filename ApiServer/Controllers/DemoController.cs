using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.Filters;
using ApiServer.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Demo")]
    [ValidateModel]
    public class DemoController : Controller
    {
        public IActionResult Post([FromBody]DemoEditModel model)
        {
            var code = 0;
            Hello(out code);

            ModelState.AddModelError("自定义", "反正就是错");
            return new ValidationFailedResult(ModelState);
            //return BadRequest(ModelState);


        }


        public void Hello(out int he)
        {
            he = 1;
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
        [Required(ErrorMessage = "Please insert id   2")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "sdfsdf")]
        public string Name { get; set; }
        [Range(1, 99, ErrorMessage = "年龄不规范")]
        public int Age { get; set; }

    }
}