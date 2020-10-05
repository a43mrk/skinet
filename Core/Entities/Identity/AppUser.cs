using Microsoft.AspNetCore.Identity;

// 163-1 Identity model
namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        
    }
}