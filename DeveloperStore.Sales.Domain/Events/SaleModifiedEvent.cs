using DeveloperStore.Sales.Domain.Events.Status;

namespace DeveloperStore.Sales.Domain.Events;

public class SaleModifiedEvent : DefautlEventInfo
{
   
    public SaleModifiedEvent(Guid saleId, Guid userId)
    {
        SaleId = saleId;
        UserId = userId;
        eventStatus = EventStatus.Sale_Update.GetDisplayName();
    }
}
