// 209
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities.OrderAggregate;
using System;

// 209 config for Order entity
namespace Infrastructure.Data.config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
       public void Configure(EntityTypeBuilder<Order> builder) 
       {
           builder.OwnsOne( o => o.ShipToAddress, a =>
           {
               a.WithOwner();
           });

           builder
            .Property(s => s.Status)
            .HasConversion(
                o => o.ToString(),
                o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
            );
            // ensures many relations and deletion of related entities.
           builder.HasMany( o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
       }
    }
}