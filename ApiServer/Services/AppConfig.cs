using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Services
{
    public class AppConfig
    {
        public IConfiguration Configuration { get; set; }
        private static AppConfig _instance = new AppConfig();
        public static AppConfig Instance { get { return _instance; } }
        AppConfig()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }



    }
}
