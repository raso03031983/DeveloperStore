using DeveloperStore.Sales.Domain.Events.Status;

namespace DeveloperStore.Sales.Domain.Events;

public class SaleCancelledEvent : DefautlEventInfo
{
    public SaleCancelledEvent(Guid saleId, Guid userId)
    {
        SaleId = saleId;
        UserId = userId;
        eventStatus = EventStatus.Sale_Cancel.GetDisplayName();
    }
}
