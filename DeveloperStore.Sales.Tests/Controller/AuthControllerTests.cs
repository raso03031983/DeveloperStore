using DeveloperStore.Sales.API.Controllers;
using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeveloperStore.Sales.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_Should_Return_Ok_When_Successful()
    {
        // Arrange
        var dto = new RegisterDto { Username = "test", Password = "123456" };
        _authServiceMock.Setup(x => x.RegisterAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Usuário registrado com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task Register_Should_Return_BadRequest_When_Exception_Thrown()
    {
        // Arrange
        var dto = new RegisterDto { Username = "test", Password = "123456" };
        _authServiceMock.Setup(x => x.RegisterAsync(dto)).ThrowsAsync(new Exception("Erro ao registrar"));

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro ao registrar", badRequest.Value);
    }

    [Fact]
    public async Task Login_Should_Return_Token_When_Valid()
    {
        // Arrange
        var dto = new LoginDto { Username = "admin", Password = "123456" };
        _authServiceMock.Setup(x => x.LoginAsync(dto)).ReturnsAsync("fake-jwt-token");

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenObj = okResult.Value?.GetType().GetProperty("token")?.GetValue(okResult.Value, null);
        Assert.Equal("fake-jwt-token", tokenObj);
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_UnauthorizedAccessException()
    {
        // Arrange
        var dto = new LoginDto { Username = "admin", Password = "wrong" };
        _authServiceMock.Setup(x => x.LoginAsync(dto)).ThrowsAsync(new UnauthorizedAccessException("Acesso negado"));

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Acesso negado", unauthorized.Value);
    }

    [Fact]
    public async Task Login_Should_Return_BadRequest_When_Generic_Exception()
    {
        // Arrange
        var dto = new LoginDto { Username = "admin", Password = "error" };
        _authServiceMock.Setup(x => x.LoginAsync(dto)).ThrowsAsync(new Exception("Erro interno"));

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Erro interno", badRequest.Value);
    }
}
