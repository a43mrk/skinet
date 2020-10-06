using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Errors;
using Infrastructure.Services;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        // 56-1 insert IServiceCollection as services at static class static method
        // to be able to be added as configuration at Startup.cs
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 172-1 Inject ITokenService and TokenService.
            // to be accessed at AccountController.
            services.AddScoped<ITokenService, TokenService>();

            // 22-3. inject Repository
            // pass the interface and the concret class.
            services.AddScoped<IProductRepository, ProductRepository>();

            // 137-3 Inject IBasketRepository and BasketRepository
            services.AddScoped<IBasketRepository,BasketRepository>();

            // 33-3 Injecting generic Repository
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            // 53-2 add error array type capability
            // custom validation error message.
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext => {
                        // get the modelState
                        var errors = actionContext.ModelState
                            .Where( e => e.Value.Errors.Count > 0)
                            .SelectMany( x => x.Value.Errors)
                            .Select(x => x.ErrorMessage).ToArray();

                        // Apply custom validation response
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