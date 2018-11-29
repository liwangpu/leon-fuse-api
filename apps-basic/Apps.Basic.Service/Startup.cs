using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.Basic.Data.Entities;
using Apps.Basic.Service.Contexts;
using Apps.Basic.Service.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Apps.Basic.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<AppConfig>(Configuration);

            #region PGSQL Setting
            services.AddEntityFrameworkNpgsql();
            var connStr = Configuration["ConnectionString"];
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));
            Console.WriteLine("数据库链接参数:{0}", connStr);
            #endregion

            #region JwtBearer Setting
            //var JwtSettingsIssuer = Configuration["JwtSettings:Issuer"];
            //Console.WriteLine("JwtSettings:Issuer参数:{0}", JwtSettingsIssuer);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtSettings:Issuer"],
                        ValidAudience = Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion

            #region Service Registry
            services.AddScoped<IRepository<Account>, AccountRepository>();
            services.AddScoped<IRepository<UserNav>, UserNavRepository>();
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<AppDbContext>();
            var appConfig= serviceProvider.GetService<AppConfig>();
            app.UseAuthentication();
            app.UseMvc();

            #region App Init
            {
                var serverId = Configuration["GuidSettings:ServerId"];
                var guidSalt = Configuration["GuidSettings:GuidSalt"];
                var guidMinLen = Configuration["GuidSettings:GuidMinLen"];
                GuidGen.Init(serverId, guidSalt, guidMinLen);

            }
            #endregion

            #region Database Init
            {
                DatabaseInitTool.InitDatabase(dbContext);
            }
            #endregion
        }
    }
}
