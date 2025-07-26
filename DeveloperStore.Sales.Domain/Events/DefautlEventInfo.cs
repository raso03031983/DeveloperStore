namespace DeveloperStore.Sales.Domain.Events
{
    public class DefautlEventInfo
    {
        public Guid SaleId { set; get; }
        public DateTime Date { set; get; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string eventStatus { get; set; }
    }
}
