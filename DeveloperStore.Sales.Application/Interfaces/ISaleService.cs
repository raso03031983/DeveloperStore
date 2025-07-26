using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Filters;

namespace DeveloperStore.Sales.Application.Interfaces
{
    public interface ISaleService
    {
        Task<Guid> CreateAsync(SaleDto saleDto, UserDto user);
        Task<SaleDto?> GetByIdAsync(Guid id);
        Task<List<SaleDto>> GetAllAsync(SaleFilter? filter = null);
        Task UpdateAsync(Guid id, SaleDto saleDto, UserDto user);
        Task CancelAsync(Guid id, UserDto user);
    }
}
