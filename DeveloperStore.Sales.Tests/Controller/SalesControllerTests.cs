using DeveloperStore.Sales.API.Controllers;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Filters;
using DeveloperStore.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

public class SalesControllerTests
{
    private readonly Mock<ISaleService> _saleServiceMock;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _saleServiceMock = new Mock<ISaleService>();
        _controller = new SalesController(_saleServiceMock.Object);
        SetupFakeUser();
    }

    private void SetupFakeUser()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "test_user")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var saleDto = new SaleDto { Items = new List<SaleItemDto>() };
        var newSaleId = Guid.NewGuid();

        _saleServiceMock.Setup(x => x.CreateAsync(saleDto, It.IsAny<UserDto>()))
                        .ReturnsAsync(newSaleId);

        var result = await _controller.Create(saleDto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var value = created.Value;

        var idProperty = value?.GetType().GetProperty("id")?.GetValue(value);

        Assert.Equal(newSaleId, idProperty);
    }


    [Fact]
    public async Task GetById_ReturnsOk_WhenSaleExists()
    {
        var saleId = Guid.NewGuid();
        var sale = new SaleDto { Id = saleId };

        _saleServiceMock.Setup(x => x.GetByIdAsync(saleId))
                        .ReturnsAsync(sale);

        var result = await _controller.GetById(saleId);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sale, ok.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenSaleNotExists()
    {
        var saleId = Guid.NewGuid();

        _saleServiceMock.Setup(x => x.GetByIdAsync(saleId))
                        .ReturnsAsync((SaleDto)null!);

        var result = await _controller.GetById(saleId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        _saleServiceMock.Setup(x => x.GetAllAsync(It.IsAny<SaleFilter>()))
                        .ReturnsAsync(new List<SaleDto>());

        var result = await _controller.GetAll(null, null, null, null, null, 1, 10, null, "asc");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<SaleDto>>(ok.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent()
    {
        var saleId = Guid.NewGuid();
        var dto = new SaleDto { Items = new List<SaleItemDto>() };

        var result = await _controller.Update(saleId, dto);

        _saleServiceMock.Verify(x => x.UpdateAsync(saleId, dto, It.IsAny<UserDto>()), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Cancel_ReturnsNoContent()
    {
        var saleId = Guid.NewGuid();

        var result = await _controller.Cancel(saleId);

        _saleServiceMock.Verify(x => x.CancelAsync(saleId, It.IsAny<UserDto>()), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    private class AnonymousType
    {
        public Guid id { get; set; }
    }
}
