using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Services;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeveloperStore.Sales.Tests.Services;

public class AuthServiceTests
{
    private IConfiguration CreateFakeConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "chave-super-segura-jwt-com-32-caracteres!!" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }

    private SalesDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SalesDbContext(options);
    }

    [Fact]
    public async Task RegisterAsync_Should_Add_New_User()
    {
        // Arrange
        var context = CreateDbContext();
        var service = new AuthService(context, CreateFakeConfiguration());

        var dto = new RegisterDto
        {
            Username = "test_user",
            Password = "123456"
        };

        // Act
        await service.RegisterAsync(dto);

        // Assert
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "test_user");
        Assert.NotNull(user);
        Assert.NotEmpty(user!.PasswordHash);
    }

    [Fact]
    public async Task RegisterAsync_Should_Throw_If_User_Already_Exists()
    {
        var context = CreateDbContext();
        context.Users.Add(new User { Id = Guid.NewGuid(), Username = "existing", PasswordHash = "hashed" });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateFakeConfiguration());

        var dto = new RegisterDto { Username = "existing", Password = "123" };

        await Assert.ThrowsAsync<Exception>(() => service.RegisterAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Token_When_Credentials_Are_Correct()
    {
        // Arrange
        var context = CreateDbContext();
        var config = CreateFakeConfiguration();
        var service = new AuthService(context, config);

        var dto = new RegisterDto { Username = "admin", Password = "123456" };
        await service.RegisterAsync(dto);

        var loginDto = new LoginDto { Username = "admin", Password = "123456" };

        // Act
        var token = await service.LoginAsync(loginDto);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Contains("ey", token); // deve conter parte do JWT
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_If_User_Not_Found()
    {
        var context = CreateDbContext();
        var service = new AuthService(context, CreateFakeConfiguration());

        var dto = new LoginDto { Username = "invalid", Password = "123" };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_If_Password_Is_Invalid()
    {
        var context = CreateDbContext();
        var config = CreateFakeConfiguration();
        var service = new AuthService(context, config);

        await service.RegisterAsync(new RegisterDto
        {
            Username = "testuser",
            Password = "validpass"
        });

        var dto = new LoginDto
        {
            Username = "testuser",
            Password = "wrongpass"
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(dto));
    }
}
