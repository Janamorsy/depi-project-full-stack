namespace NileCareAPI.Models
{
    public class BookingItem
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string InternalReference { get; set; } = string.Empty;
    }
}
