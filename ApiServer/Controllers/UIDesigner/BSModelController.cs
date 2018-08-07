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
                Fields = new List<BSModelField>()
                {
                    new BSModelField(){ Id="Icon",Name="Icon",Width=85}
                    ,new BSModelField(){ Id="Name",Name="Name",Width=125}
                    , new BSModelField(){ Id="Description",Name="Description",Width=185}
                },
                PageSizeOptions = new List<int>() { 15, 25, 500 }
            };
            return Ok(model);
        }
    }



    class BSModelField
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Width { get; set; }
        public string Expression { get; set; }

        //    id: string;
        //name: string;
        //description: string;
        //width: number;
        //expression: string;
    }
}