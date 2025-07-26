using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Events;
using DeveloperStore.Sales.Application.Filters;
using DeveloperStore.Sales.Application.Interfaces;
using DeveloperStore.Sales.Application.Services;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Moq;

public class SaleServiceTests
{
    private readonly SalesDbContext _context;
    private readonly Mock<IDomainEventPublisher> _eventPublisherMock;
    private readonly Mock<ISaleNumberGenerator> _numberGeneratorMock;
    private readonly SaleService _service;

    public SaleServiceTests()
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SalesDbContext(options);
        _eventPublisherMock = new Mock<IDomainEventPublisher>();
        _numberGeneratorMock = new Mock<ISaleNumberGenerator>();

        _numberGeneratorMock.Setup(x => x.GenerateNextAsync())
                            .ReturnsAsync("SALE-001");

        _service = new SaleService(_context, _eventPublisherMock.Object, _numberGeneratorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSaleAndPublishEvent()
    {
        var dto = new SaleDto
        {
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Cliente Teste",
            BranchId = Guid.NewGuid(),
            BranchName = "Filial A",
            Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Produto 1",
                    Quantity = 2,
                    UnitPrice = 100,
                    Discount = 10
                }
            }
        };

        var user = new UserDto { UserId = Guid.NewGuid() };

        var saleId = await _service.CreateAsync(dto, user);

        var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(x => x.Id == saleId);

        Assert.NotNull(sale);
        Assert.Equal(dto.CustomerName, sale.CustomerName);
        _eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SaleCreatedEvent>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSaleDto_WhenExists()
    {
        // Arrange: cria venda
        var sale = new DeveloperStore.Sales.Domain.Entities.Sale("SALE-002", DateTime.UtcNow, Guid.NewGuid(), "Cliente", Guid.NewGuid(), "Filial");
        sale.AddItem(new DeveloperStore.Sales.Domain.Entities.SaleItem(sale.Id, Guid.NewGuid(), "Produto", 1, 100, 0));
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(sale.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sale.SaleNumber, result.SaleNumber);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilteredSales()
    {
        _context.Sales.Add(new DeveloperStore.Sales.Domain.Entities.Sale("SALE-003", DateTime.UtcNow, Guid.NewGuid(), "Cliente XYZ", Guid.NewGuid(), "Filial A"));
        await _context.SaveChangesAsync();

        var filter = new SaleFilter
        {
            CustomerName = "Cliente",
            PageNumber = 1,
            PageSize = 10
        };

        var result = await _service.GetAllAsync(filter);

        Assert.Single(result);
        Assert.Contains(result, x => x.CustomerName.Contains("Cliente"));
    }

    [Fact]
    public async Task UpdateAsync_PublishesModifiedEvent()
    {
        var sale = new DeveloperStore.Sales.Domain.Entities.Sale("SALE-004", DateTime.UtcNow, Guid.NewGuid(), "João", Guid.NewGuid(), "Matriz");
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        var dto = new SaleDto { Items = new() };
        var user = new UserDto { UserId = Guid.NewGuid() };

        await _service.UpdateAsync(sale.Id, dto, user);

        _eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SaleModifiedEvent>()), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_ShouldCancelSaleAndItems_AndPublishEvents()
    {
        var sale = new DeveloperStore.Sales.Domain.Entities.Sale("SALE-005", DateTime.UtcNow, Guid.NewGuid(), "Maria", Guid.NewGuid(), "Loja 1");
        sale.AddItem(new DeveloperStore.Sales.Domain.Entities.SaleItem(sale.Id, Guid.NewGuid(), "Produto A", 1, 100, 0));
        sale.AddItem(new DeveloperStore.Sales.Domain.Entities.SaleItem(sale.Id, Guid.NewGuid(), "Produto B", 2, 50, 0));
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        var user = new UserDto { UserId = Guid.NewGuid() };

        await _service.CancelAsync(sale.Id, user);

        var updated = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == sale.Id);
        Assert.True(updated.Cancelled);
        Assert.All(updated.Items, i => Assert.True(i.Cancelled));

        _eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<ItemCancelledEvent>()), Times.Exactly(2));
    }
}
