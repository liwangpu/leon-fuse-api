using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apps.Base.Common.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apps.OMS.Service.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MemberController : ServiceBaseController<AssetCategory>
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}