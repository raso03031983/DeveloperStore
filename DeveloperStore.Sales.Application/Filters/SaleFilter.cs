namespace DeveloperStore.Sales.Application.Filters;

public class SaleFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public bool? Cancelled { get; set; }

    // Paginação
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Ordenação
    public string? OrderBy { get; set; }
    public string SortDirection { get; set; } = "asc";
}
