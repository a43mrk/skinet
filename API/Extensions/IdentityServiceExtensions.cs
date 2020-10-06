using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// 166-1
namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();

            // don't forget to add authentication
            // 171-1
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ValidateIssuerSigningKey should be true
                    // validateIssuerSigningKey: false, allows anonymous users to access everything.
                    ValidateIssuerSigningKey = true,
                    // 171-3 add key:value into appsettings.Development.json
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    // ValidateIssuer should be true
                    ValidateIssuer = true,

                    // 173
                    // Validate is null. (is got when audience defaults is used.)
                    // A token can have a audience(client application).
                    ValidateAudience = false,
                };
            });

            return services;
        }
    }
}