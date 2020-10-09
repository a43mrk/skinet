using System.Linq;
using System.Security.Claims;

// 214- extensions for principal
namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault( x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}