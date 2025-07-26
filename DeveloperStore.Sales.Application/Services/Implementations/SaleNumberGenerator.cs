using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Application.Services;

public class SaleNumberGenerator : ISaleNumberGenerator
{
    private readonly SalesDbContext _context;

    public SaleNumberGenerator(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextAsync()
    {
        var currentYear = DateTime.UtcNow.Year;

        var countThisYear = await _context.Sales.CountAsync(s => s.Date.Year == currentYear);

        var nextNumber = countThisYear + 1;

        return $"VEN-{currentYear}-{nextNumber:D4}";
    }
}
