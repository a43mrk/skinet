using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SkiNet API", Version = "v1"});
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            // Swagger middleware shold be set after authorization!
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1"); });

            return app;
        }
    }
}