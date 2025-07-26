using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Events;
using DeveloperStore.Sales.Application.Filters;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Application.Services;

public class SaleService : ISaleService
{
    private readonly SalesDbContext _context;
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly ISaleNumberGenerator _saleNumberGenerator;

    public SaleService(SalesDbContext context, 
                       IDomainEventPublisher eventPublisher,
                       ISaleNumberGenerator saleNumberGenerator)
    {
        _context = context;
        _eventPublisher = eventPublisher;
        _saleNumberGenerator = saleNumberGenerator;
    }

    public async Task<Guid> CreateAsync(SaleDto dto, UserDto user)
    {
        var generatedNumber = await _saleNumberGenerator.GenerateNextAsync();

        var sale = new Sale(
            saleNumber: generatedNumber,
            date: dto.Date,
            customerId: dto.CustomerId,
            customerName: dto.CustomerName,
            branchId: dto.BranchId,
            branchName: dto.BranchName
        );

        foreach (var item in dto.Items)
        {
            var saleItem = new SaleItem(
                sale.Id,
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Discount
            );
            sale.AddItem(saleItem);
        }

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        await _eventPublisher.PublishAsync(new SaleCreatedEvent(sale.Id, user.UserId));
        return sale.Id;
    }

    public async Task<SaleDto?> GetByIdAsync(Guid id)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null) return null;

        return new SaleDto
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Date = sale.Date,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            Cancelled = sale.Cancelled,
            Items = sale.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount
            }).ToList()
        };
    }

    public async Task<List<SaleDto>> GetAllAsync(SaleFilter? filter = null)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        if (filter != null)
        {
            if (filter.StartDate.HasValue)
                query = query.Where(s => s.Date >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(s => s.Date <= filter.EndDate.Value);

            if (!string.IsNullOrEmpty(filter.CustomerName))
                query = query.Where(s => s.CustomerName.Contains(filter.CustomerName));

            if (!string.IsNullOrEmpty(filter.BranchName))
                query = query.Where(s => s.BranchName.Contains(filter.BranchName));

            if (filter.Cancelled.HasValue)
                query = query.Where(s => s.Cancelled == filter.Cancelled.Value);
        }

        // Ordenação dinâmica
        if (!string.IsNullOrEmpty(filter.OrderBy))
        {
            var isDescending = filter.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

            query = filter.OrderBy.ToLower() switch
            {
                "date" => isDescending ? query.OrderByDescending(s => s.Date) : query.OrderBy(s => s.Date),
                "customername" => isDescending ? query.OrderByDescending(s => s.CustomerName) : query.OrderBy(s => s.CustomerName),
                "totalamount" => isDescending ? query.OrderByDescending(s => s.TotalAmount) : query.OrderBy(s => s.TotalAmount),
                _ => query.OrderByDescending(s => s.Date) // padrão
            };
        }
        else
        {
            query = query.OrderByDescending(s => s.Date); // padrão
        }

        // Paginação
        query = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);


        return await query.Select(s => new SaleDto
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            Date = s.Date,
            CustomerId = s.CustomerId,
            CustomerName = s.CustomerName,
            BranchId = s.BranchId,
            BranchName = s.BranchName,
            TotalAmount = s.TotalAmount,
            Cancelled = s.Cancelled,
            Items = s.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount
            }).ToList()
        }).ToListAsync();
    }

    public async Task UpdateAsync(Guid id, SaleDto dto, UserDto user)
    {
        var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        if (sale == null) throw new Exception("Venda não encontrada.");

        await _eventPublisher.PublishAsync(new SaleModifiedEvent(id, user.UserId));
        await _context.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id, UserDto user)
    {
        var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        if (sale == null) throw new Exception("Venda não encontrada.");

        sale.Cancel();
        foreach (var item in sale.Items) {
            item.Cancel();
            await _eventPublisher.PublishAsync(new ItemCancelledEvent(sale.Id, item.ProductId, user.UserId));
        }

        await _context.SaveChangesAsync();
    }
}
