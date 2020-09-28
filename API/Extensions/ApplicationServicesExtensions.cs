using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Errors;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 22-3. inject Repository
            // pass the interface and the concret class.
            services.AddScoped<IProductRepository, ProductRepository>();

            // Injecting generic type
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

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
            return services;
        }
        
    }
}