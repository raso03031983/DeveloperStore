using DeveloperStore.Sales.Application.Services;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Tests.Services;

public class SaleNumberGeneratorTests
{
    private SalesDbContext CreateDbContextWithSales(int existingSalesThisYear)
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new SalesDbContext(options);

        // Seed sales for current year
        var currentYear = DateTime.UtcNow.Year;
        for (int i = 1; i <= existingSalesThisYear; i++)
        {
            var sale = new Sale(
                saleNumber: $"VEN-{currentYear}-{i:D4}",
                date: new DateTime(currentYear, 1, 1).AddDays(i),
                customerId: Guid.NewGuid(),
                customerName: $"Cliente {i}",
                branchId: Guid.NewGuid(),
                branchName: $"Filial {i}"
            );

            context.Sales.Add(sale);
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GenerateNextAsync_Should_Generate_Formatted_SaleNumber()
    {
        // Arrange
        var context = CreateDbContextWithSales(existingSalesThisYear: 3);
        var generator = new SaleNumberGenerator(context);

        // Act
        var result = await generator.GenerateNextAsync();

        // Assert
        var currentYear = DateTime.UtcNow.Year;
        Assert.Equal($"VEN-{currentYear}-0004", result);
    }

    [Fact]
    public async Task GenerateNextAsync_Should_Start_At_0001_If_No_Sales()
    {
        var context = CreateDbContextWithSales(0);
        var generator = new SaleNumberGenerator(context);

        var result = await generator.GenerateNextAsync();

        var currentYear = DateTime.UtcNow.Year;
        Assert.Equal($"VEN-{currentYear}-0001", result);
    }
}
