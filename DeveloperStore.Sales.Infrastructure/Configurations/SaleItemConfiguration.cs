using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.SaleId)
               .IsRequired();

        builder.Property(i => i.ProductId)
               .IsRequired();

        builder.Property(i => i.ProductName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(i => i.Quantity)
               .IsRequired();

        builder.Property(i => i.UnitPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(i => i.Discount)
               .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Total)
               .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Cancelled)
               .IsRequired();
    }
}
