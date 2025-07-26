namespace DeveloperStore.Sales.Application.Interfaces
{
    public interface ISaleNumberGenerator
    {
        Task<string> GenerateNextAsync();
    }

}
