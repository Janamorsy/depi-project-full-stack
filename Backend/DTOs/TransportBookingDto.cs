namespace NileCareAPI.DTOs;

public class TransportBookingDto
{
    public int Id { get; set; }
    public int TransportId { get; set; }
    public string VehicleType { get; set; } = string.Empty;
    public DateTime PickupDateTime { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public int NumberOfPassengers { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = "Unpaid";
}

public class CreateTransportBookingDto
{
    public int TransportId { get; set; }
    public DateTime PickupDateTime { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public int NumberOfPassengers { get; set; }
    public string SpecialRequests { get; set; } = string.Empty;
}


