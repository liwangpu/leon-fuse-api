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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;


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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opts =>
            {
                // Force Camel Case to JSON
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //ignore Entity framework Navigation property back reference problem. Blog >> Posts. Post >> Blog. Blog.post.blog will been ignored.
                opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.Configure<AppConfig>(Configuration);

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

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
            services.AddScoped<IRepository<Account>, AccountRepository>();
            services.AddScoped<IRepository<UserNav>, UserNavRepository>();
            //services.AddScoped<IRepository<FileAsset>, FileRepository>();
            services.AddScoped<IRepository<Navigation>, NavigationRepository>();
            services.AddScoped<IRepository<UserRole>, UserRoleRepository>();
            services.AddScoped<IRepository<Organization>, OrganizationRepository>();
            services.AddScoped<IRepository<OrganizationType>, OrganizationTypeRepository>();
            services.AddScoped<ITreeRepository<OrganizationTree>, OrganizationTreeRepository>();
            services.AddScoped<IRepository<Department>, DepartmentRepository>();
            //services.add
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
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
    }
}
