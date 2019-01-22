using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Apps.Base.Common.Models;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Flurl;
using Flurl.Http;
using System.Linq;
using Newtonsoft.Json;
namespace Apps.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine("---- finished ----");
            Console.ReadKey();
        }

        static async Task MainAsync(string[] args)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var jsonPath = Path.Combine(dir, "swagger.json");
            if (File.Exists(jsonPath))
            {
                var str = File.ReadAllText(jsonPath);
                var obj = JsonConvert.DeserializeObject<SwaggerMapping>(str);

                foreach (var item in obj.paths)
                {
                    var b = item.Value;

                }

                var a = 1;
            }
        }
    }



    class SwaggerMapping
    {
        public Dictionary<string, APIRoute> paths { get; set; }
    }

    class APIRoute
    {
        public HttpMethod get { get; set; }
        public HttpMethod post { get; set; }
        public HttpMethod put { get; set; }
        public HttpMethod delete { get; set; }
    }


    class HttpMethod
    {
        public string summary { get; set; }
        public List<HttpParameter> parameters { get; set; }
    }

    class HttpParameter
    {
        public string name { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
    }
}
