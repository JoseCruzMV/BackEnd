using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.OrderAddress, oa =>
        {
            oa.WithOwner();
        });

        builder
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.Status)
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v ?? ""));
                
    }
}