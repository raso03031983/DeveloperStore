using DeveloperStore.Sales.Domain.Events.Status;

namespace DeveloperStore.Sales.Domain.Events;

public class SaleCreatedEvent : DefautlEventInfo
{
    public SaleCreatedEvent(Guid saleId,Guid userId)
    {
        SaleId = saleId;
        UserId = userId;
        eventStatus = EventStatus.Sale_Create.GetDisplayName();
    }
}
