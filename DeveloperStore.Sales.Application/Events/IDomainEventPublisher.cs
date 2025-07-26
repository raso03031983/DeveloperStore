namespace DeveloperStore.Sales.Application.Events
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<T>(T domainEvent) where T : class;
    }
}
