using DeveloperStore.Sales.Domain.Events.Status;

namespace DeveloperStore.Sales.Domain.Events;

public class ItemCancelledEvent : DefautlEventInfo
{
    public Guid ProductId { get; }

    public ItemCancelledEvent(Guid saleId, Guid productId, Guid userId)
    {
        SaleId = saleId;
        ProductId = productId;
        UserId = userId;
        eventStatus = EventStatus.Sale_Item_Update.GetDisplayName();
    }
}
