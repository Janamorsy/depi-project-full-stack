namespace NileCareAPI.Models;

public class Appointment
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string? DoctorUserId { get; set; }
    public DoctorUser? DoctorUser { get; set; }
    public int? LegacyDoctorId { get; set; }
    public Doctor? LegacyDoctor { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string PatientNotes { get; set; } = string.Empty;
    public string DoctorNotes { get; set; } = string.Empty;
    public string AppointmentType { get; set; } = "In-Person";
    public decimal ConsultationFee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string PaymentStatus { get; set; } = "Unpaid";
    public int QueueNumber { get; set; } = 0;
}


