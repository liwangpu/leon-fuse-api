using Apps.FileSystem.Service.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.FileSystem.Service
{
    public class DatabaseInitTool
    {
        /// <summary>
        /// 应用备份文件夹
        /// </summary>
        public const string BackupFoler = "AppBackup";

        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="context"></param>
        public static void InitDatabase(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, AppDbContext context)
        {
           
        }
    }
}
