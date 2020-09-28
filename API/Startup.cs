using Microsoft.AspNetCore.Builder;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using API.Helpers;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using API.Errors;
using System.Linq;
using Microsoft.OpenApi.Models;
using API.Extensions;

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
        // public IConfiguration Configuration { get; }

        // *The dependency injection container.
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // need to specify where the mapping profile are located(the Assembly where was created the mapping profile class).
            services.AddAutoMapper(typeof(MappingProfiles));

            // 12-2 add connection string after set the key value at appsettings.Development.json (12-1)
            // GetConnectionString points to GetSection("ConnectionStrings")[name]
            // name should be the key that returns the connection string!
            services.AddDbContext<StoreContext>(x =>
                // look for DefaultConnection key at json to get the connection string.
                x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddApplicationServices();
            services.AddSwaggerDocumentation();

            // 67-1 adding Cors
            services.AddCors( opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // Register the middlewares here!
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            // replace original exception middleware by ours.
            app.UseMiddleware<ExceptionMiddleware>();

            // repass status code to endpoint /errors/{statusCode}
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            // serve static files
            app.UseStaticFiles();

            // 66-2 add cors just before authorization.
            // beware of misspelling.
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            // swagger
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
