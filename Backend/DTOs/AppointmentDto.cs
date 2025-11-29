namespace NileCareAPI.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public string? PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? PatientPhoneNumber { get; set; }
    public string? DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorPhoneNumber { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PatientNotes { get; set; } = string.Empty;
    public string DoctorNotes { get; set; } = string.Empty;
    public string AppointmentType { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string PaymentStatus { get; set; } = "Unpaid";
    public int QueueNumber { get; set; }
    public int TotalInSlot { get; set; }
}


