namespace NileCareAPI.Models;

public class TransportBooking
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public int TransportId { get; set; }
    public Transport Transport { get; set; } = null!;
    public DateTime PickupDateTime { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public int NumberOfPassengers { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public string SpecialRequests { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string PaymentStatus { get; set; } = "Unpaid"; // Recommended default value
 }


