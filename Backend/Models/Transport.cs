namespace NileCareAPI.Models
{
    // Ensure this namespace matches your project structure
    public class Transport
    {
        public int Id { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public bool WheelchairAccessible { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerHour { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Features { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}

