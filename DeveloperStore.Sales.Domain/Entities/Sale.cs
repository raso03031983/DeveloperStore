namespace DeveloperStore.Sales.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime Date { get; private set; }

    // External Identity - Cliente
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }

    // External Identity - Filial
    public Guid BranchId { get; private set; }
    public string BranchName { get; private set; }

    public decimal TotalAmount { get; private set; }
    public bool Cancelled { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();


    protected Sale() { }

    public Sale(string saleNumber, DateTime date, Guid customerId, string customerName, Guid branchId, string branchName)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        Cancelled = false;
    }

    public void AddItem(SaleItem item)
    {
        _items.Add(item);
        RecalculateTotal();
    }

    public void Cancel()
    {
        Cancelled = true;
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Where(i => !i.Cancelled).Sum(i => i.Total);
    }
}
