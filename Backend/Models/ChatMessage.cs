namespace NileCareAPI.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public ApplicationUser Patient { get; set; } = null!;
    public string DoctorId { get; set; } = string.Empty;
    public DoctorUser Doctor { get; set; } = null!;
    public string? FileUrl { get; set; } // Add this

    public string Message { get; set; } = string.Empty;
    public string SenderType { get; set; } = "Patient";
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}


