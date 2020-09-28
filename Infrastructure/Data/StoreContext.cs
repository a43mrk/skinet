using Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

// 11-1 create StoreContext to use EF.
// install Microsoft.EntityFrameworkCore (use the same version on host version at dotnet info)
// install Microsoft.EntityFrameworkcore.Sqlite
// > dotnet restore
namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        // 11-2 create a contructor w/ DbContextOptions as parameter.
        // don't forget to pass options to base class.
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        // 11-3 Declare the Entities here as DbSet of that Entity.
        // property should be the plurals name of the entity.
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        // 26-2 register the EF Migration Configuration.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion<double>();
                    }
                }
            }
        }
    }
}