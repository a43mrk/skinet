using Microsoft.EntityFrameworkCore;
using Core.Entities.OrderAggregate;

// 209 config for OrderItem entity
namespace Infrastructure.Data.config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne( i => i.ItemOrdered, io => { io.WithOwner(); });
            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2");
        }
    }
}