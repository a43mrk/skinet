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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            // Injecting generic type
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddControllers();

            // need to specify where the mapping profile are located(the Assembly where was created the mapping profile class).
            services.AddAutoMapper(typeof(MappingProfiles));

            // GetConnectionString points to GetSection("ConnectionStrings")[name]
            // name should be the key that returns the connection string!
            services.AddDbContext<StoreContext>(x =>
                x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            // add error array type capability
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext => {
                        var errors = actionContext.ModelState
                            .Where( e => e.Value.Errors.Count > 0)
                            .SelectMany( x => x.Value.Errors)
                            .Select(x => x.ErrorMessage).ToArray();
                        var errorResponse = new ApiValidationErrorResponse {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(errorResponse);
                };
            });

            // Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1"});
            });
        }

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

            app.UseAuthorization();

            // Swagger middleware shold be set after authorization!
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
