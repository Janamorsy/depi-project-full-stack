namespace NileCareAPI.Models;

public class Payment
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string StripeSessionId { get; set; } = string.Empty;
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "egp";
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    public string BookingType { get; set; } = string.Empty; // Appointment, Hotel, Transport
    public int BookingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
