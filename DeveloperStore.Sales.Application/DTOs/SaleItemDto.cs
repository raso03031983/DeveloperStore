using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Sales.Application.DTOs;

public class SaleItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }

}
