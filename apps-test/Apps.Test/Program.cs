using System;
using System.Threading.Tasks;
using Apps.Basic.Export.DTOs;
using Apps.Basic.Export.Models;
using Flurl;
using Flurl.Http;

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
            var serverBase = "http://testapi.damaozhu.com.cn";
            //var serverBase = "localhsot:1882";

            var tokenUrl = $"{serverBase}/token";
            var res = await tokenUrl.PostJsonAsync(new TokenRequestModel() { Account = "omega", Password = "e10adc3949ba59abbe56e057f20f883e" }).ReceiveJson<TokenDTO>();

            //var r =await res.Content.ReadAsStringAsync();
     

        }
    }
}
