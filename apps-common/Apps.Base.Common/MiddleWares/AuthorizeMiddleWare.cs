using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
namespace Apps.Base.Common.MiddleWares
{
    class TokenRequestModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }

    public class AuthorizeMiddleWare
    {
        public static Func<HttpContext, Func<Task>, Task> Authorize(string apiGetway)
        {
            //使用该中间件,APIGateway必须配置
            if (string.IsNullOrWhiteSpace(apiGetway))
                throw new Exception("APIGateway Setting is empty!");

            var auth = new Func<HttpContext, Func<Task>, Task>(async (context, next) =>
            {
                var url = context.Request.Path.ToString().ToLower();
                if (url.Contains("/token"))
                {
                    var req = context.Request;

                    var tokenUrl = $"{apiGetway}/token";
                    using (var reader = new StreamReader(req.Body))
                    {
                        var body = reader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<TokenRequestModel>(body);
                        var respond = await tokenUrl.WithHeaders(new { Content_Type = "application/json" }).PostJsonAsync(data).ReceiveString();
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(respond);
                    }
                    return;
                }

                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
            return auth;
        }
    }
}
