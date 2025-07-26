using DeveloperStore.Sales.API.Models;
using DeveloperStore.Sales.Application.DTOs;

namespace DeveloperStore.Sales.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(RegisterDto registerDto);
    }
}
