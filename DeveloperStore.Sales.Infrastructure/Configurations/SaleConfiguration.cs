using DeveloperStore.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeveloperStore.Sales.Infrastructure.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(s => s.Date)
               .IsRequired();

        builder.Property(s => s.CustomerId)
               .IsRequired();

        builder.Property(s => s.CustomerName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.BranchId)
               .IsRequired();

        builder.Property(s => s.BranchName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.TotalAmount)
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Cancelled)
               .IsRequired();

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(i => i.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
