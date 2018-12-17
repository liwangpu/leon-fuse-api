using Apps.Base.Common;
using Apps.Base.Common.Interfaces;
using Apps.FileSystem.Data.Entities;
using Apps.FileSystem.Service.Contexts;
using Apps.FileSystem.Service.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace Apps.FileSystem.Service
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
            Console.WriteLine("AppSetting=>ConnectionString:{0}", connStr);
            #endregion

            #region JwtBearer Setting
            var jwtSettingsIssuer = Configuration["JwtSettings:Issuer"];
            var audience = Configuration["JwtSettings:Audience"];
            var secretKey = Configuration["JwtSettings:SecretKey"];
            Console.WriteLine("AppSetting=>JwtSettings:Issuer:{0}", jwtSettingsIssuer);
            Console.WriteLine("AppSetting=>JwtSettings:Audience:{0}", audience);
            Console.WriteLine("AppSetting=>JwtSettings:SecretKey:{0}", secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettingsIssuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region Service Registry
            services.AddScoped<IRepository<FileAsset>, FileRepository>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            hostStaticFileServer(app, env);
            var dbContext = serviceProvider.GetService<AppDbContext>();
            var appConfig = serviceProvider.GetService<AppConfig>();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseMvc();

            #region App Init
            {
                var serverId = Configuration["GuidSettings:ServerId"];
                var guidSalt = Configuration["GuidSettings:GuidSalt"];
                var guidMinLen = Configuration["GuidSettings:GuidMinLen"];
                Console.WriteLine("AppSetting=>GuidSettings:ServerId:{0}", serverId);
                Console.WriteLine("AppSetting=>GuidSettings:GuidSalt:{0}", guidSalt);
                Console.WriteLine("AppSetting=>GuidSettings:GuidMinLen:{0}", guidMinLen);
                GuidGen.Init(serverId, guidSalt, guidMinLen);
            }
            #endregion

            #region Database Init
            {
                dbContext.Database.Migrate();
                DatabaseInitTool.InitDatabase(app, env, serviceProvider, dbContext);
            }
            #endregion
        }

        public void hostStaticFileServer(IApplicationBuilder app, IHostingEnvironment env)
        {
            Console.WriteLine("content root path: " + env.ContentRootPath);
            Console.WriteLine("web root path: " + env.WebRootPath);
            if (Directory.Exists(env.WebRootPath) == false)
            {
                if (string.IsNullOrEmpty(env.WebRootPath))
                {
                    env.WebRootPath = Path.Combine(env.ContentRootPath, "wwwroot");
                }
                Console.WriteLine("web root path not exist. create " + env.WebRootPath);
                Directory.CreateDirectory(env.WebRootPath);
            }
            string uploadPath = Path.Combine(env.WebRootPath, "upload");
            Console.WriteLine("uploadpath: " + uploadPath);
            if (Directory.Exists(uploadPath) == false)
            {
                Console.WriteLine("upload path not exist. create " + uploadPath);
                Directory.CreateDirectory(uploadPath);
            }

            // default wwwroot directory
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    if (ctx.Context.Response.Headers.ContainsKey("Content-Type") == false)
                        ctx.Context.Response.Headers.Add("Content-Type", "application/octet-stream");
                }
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    if (ctx.Context.Response.Headers.ContainsKey("Content-Type") == false)
                        ctx.Context.Response.Headers.Add("Content-Type", "application/octet-stream");
                },
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadPath),
                RequestPath = "/upload"
            });
        }
    }
}
