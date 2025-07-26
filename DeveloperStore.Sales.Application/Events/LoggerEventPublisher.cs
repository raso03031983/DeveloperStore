using Microsoft.Extensions.Logging;

namespace DeveloperStore.Sales.Application.Events;

public class LoggerEventPublisher : IDomainEventPublisher
{
    private readonly ILogger<LoggerEventPublisher> _logger;

    public LoggerEventPublisher(ILogger<LoggerEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T domainEvent) where T : class
    {
        _logger.LogInformation("Evento publicado: {@Event}", domainEvent);
        return Task.CompletedTask;
    }
}
