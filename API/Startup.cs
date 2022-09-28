using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.RedisConfig;
using Core.Utilities.Security;
using Core.Utilities.Security.JWT;
using DAL.Abstract;
using DAL.Concrete.EntityFramework;
using DAL.Concrete.Redis;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
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

            var tokenOptions = Configuration.GetSection("TokenSettings").Get<TokenOptionsDTO>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddSingleton<IRedisConfig, RedisConfig>();
            services.AddSingleton<IRedisCacheService, RedisCacheManager>();

            services.AddScoped<IUserDAL, EFUserDAL>();
            services.AddScoped<IBookDAL, EFBookDAL>();
            services.AddScoped<IRoleDAL, EFRoleDAL>();
            services.AddScoped<IReservationDAL, EFReservationDAL>();
            services.AddScoped<ICompanyDAL, EFCompanyDAL>();

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IBookService, BookManager>();
            services.AddScoped<IRoleService, RoleManager>();
            services.AddScoped<IReservationService, ReservationManager>();
            services.AddScoped<ICompanyService, CompanyManager>();

            services.AddScoped<ITokenHelper, JwtHelper>();

            services.AddScoped<IWebRequestService, WebRequestManager>();

            services.AddScoped<IFileService, FileManager>();


            services.AddDependencyResolvers(new ICoreModule[]
          {
                new CoreModule()
          });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidIssuer = tokenOptions.Issuer,
                       ValidAudience = tokenOptions.Audience,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.Token)
                   };
               });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
