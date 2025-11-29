namespace NileCareAPI.DTOs;

public class BookAppointmentDto
{
    public string? DoctorUserId { get; set; }
    public int? LegacyDoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string PatientNotes { get; set; } = string.Empty;
    public string AppointmentType { get; set; } = "In-Person";
}


