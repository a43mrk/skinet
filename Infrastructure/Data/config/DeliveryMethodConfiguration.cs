using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 209 config for DeliveryMethod entity
namespace Infrastructure.Data.config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Price).HasColumnType("decimal(18,3)");
        }
    }
}