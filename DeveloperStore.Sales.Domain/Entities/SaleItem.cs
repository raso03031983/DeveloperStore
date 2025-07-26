namespace DeveloperStore.Sales.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid SaleId { get; private set; }

    // External Identity - Produto
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }
    public bool Cancelled { get; private set; }

    protected SaleItem() { }

    public SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice, decimal discount)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Cancelled = false;

        Total = CalculateTotal();
    }

    public void Cancel()
    {
        Cancelled = true;
        Total = 0;
    }

    private decimal CalculateTotal()
    {
        var subtotal = UnitPrice * Quantity;
        return subtotal - Discount;
    }
}
