using Microsoft.AspNetCore.Builder;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using API.Middleware;
using API.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace API
{
    public class Startup
    {
        // underline as convention for private properties.
        // private readonly IConfiguration configuration;
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
            // this.configuration = configuration;
            // removed because now he injects to an private property.
            // Configuration = configuration;
        } 

        // removed by tutor. because he don't likes!
        // public Startup(IConfiguration configuration) 
        // {
        //     this.Configuration = configuration;
               
        // }
        // public IConfiguration Configuration { get; }

        // 294
        public void ConfigurationDevelopmentServices(IServiceCollection services)
        {
            // 12-2 add connection string after set the key value at appsettings.Development.json (12-1)
            // GetConnectionString points to GetSection("ConnectionStrings")[name]
            // name should be the key that returns the connection string!
            services.AddDbContext<StoreContext>(x =>
                // look for DefaultConnection key at json to get the connection string.
                x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            // 163-2 Inject Identity DbContext
            // 163-3 Add conection string and the db name at appsettings.Development.json
            // 164-1 Create a new migration for identity
            // dotnet ef migrations add IdentityInitial -p Infrastructure -s API -o Identity/Migrations -c AppIdentityDbContext
            //                          ^-name             ^-project          ^-starter class      ^-output        ^-DbContext
            // to remove the last migration, !only execute this if you didn't executed database update.
            // dotnet ef migrations remove -p Infrastructure -s API -c AppIdentityDbContext
            services.AddDbContext<AppIdentityDbContext>( x =>
            {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }

        // 294
        public void ConfigurationProductionServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>(x =>
                x.UseMySql(_config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>( x =>
            {
                x.UseMySql(_config.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }

        // *The dependency injection container.
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // 43-3 add AutoMapper as Services
            // need to specify where the mapping profile are located(the Assembly where was created the mapping profile class).
            // pass the Mapping profile type to AddAutoMapper.
            services.AddAutoMapper(typeof(MappingProfiles));

            // 135-1
            // Redis configuration
            // 135-2 add "Redis": "localhost" key:value to appsettings.Development.json
            services.AddSingleton<IConnectionMultiplexer>( c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            // 56-2 inject the custom services
            services.AddApplicationServices();

            // 166-2
            // 171-2 pass IConfiguration into IdentityServiceExtension
            services.AddIdentityServices(_config);

            // 56-4 inject the custom swagger service
            services.AddSwaggerDocumentation();

            // 67-1 adding Cors
            services.AddCors( opt => {
                // beware to not misspell CorsPolicy with camelcase
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // Register the middlewares here!
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // original exception handler
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            // 52-3 replace original exception handler middleware by ours.
            app.UseMiddleware<ExceptionMiddleware>();

            // 51-2 repass status code to endpoint /errors/{statusCode}
            // non existing routes error will be redirected to error controller to be handled.
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            // 46-1 serve static files
            app.UseStaticFiles();

            //290 new static folder
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ), RequestPath = "/content"
            });

            // 67-2 add cors just before authorization.
            // beware of misspelling.
            app.UseCors("CorsPolicy");

            // 171-4 add just before UseAuthorization the UseAuthentication
            app.UseAuthentication();
            app.UseAuthorization();

            // 56-5 add swagger custom middleware class config after UseAuthorization.
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // 290 API needs to know endpoints
                // prevent to find routes for angular content
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
