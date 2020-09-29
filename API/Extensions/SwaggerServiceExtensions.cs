using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        // 56-2 extends IServiceCollection as service on a static class static method to be able to add at
        // Startup.ConfigureServices
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1"});
            });

            return services;
        }

        // 56-3 extends IApplicationBuilder at static class's static method to be able to
        // be added as middleware at Startup.Configure
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            // Swagger middleware shold be set after authorization!
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1"); });

            return app;
        }
    }
}