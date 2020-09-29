using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Text.Json;
using API.Errors;

// 52-2 Middleware to handle Exceptions.
namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // RequestDelegate handle http requests
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _env = environment;
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // only proceed with the request if no exception ocurred.
                await _next(context);
            }
            catch (Exception ex)
            {
                // log the exception to console
                _logger.LogError(ex, ex.Message);

                // set request context that will be the clients response.
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                // depending on the running profile the response will be less verbose for production and
                // verbose at development mode.
                var response = _env.IsDevelopment() 
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                // 52-4 change exception stacktrace message to camelcase.
                var options = new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // 52-5 pass the camelcase options to JsonSerializer.
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);

            }
        }
    }
}