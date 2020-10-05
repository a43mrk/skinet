using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    // 163-1 Identity DbContext
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        // DbContextOptions parameter should be of type AppIdentityDbContext.
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
            // Add DbSet's here!
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}