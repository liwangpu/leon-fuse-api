using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Apps.Base.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                     //if()
                     config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
                     config.AddCommandLine(args);
                 })
                 .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
#if DEBUG
                .UseUrls("http://*:1881")
#endif
                .UseNLog()
                .UseKestrel(options =>
                {
                    //最大文件上传3G
                    options.Limits.MaxRequestBodySize = 3 * 1024 * 1024 * 1024L;
                })
                .Build();
        }
    }
}
