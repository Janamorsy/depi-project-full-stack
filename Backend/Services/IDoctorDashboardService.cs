using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IDoctorDashboardService
{
    Task<List<AppointmentDto>> GetDoctorAppointmentsAsync();
    Task<MedicalProfileDto?> GetPatientProfileAsync(string patientId);
    Task UpdateAppointmentNotesAsync(int appointmentId, string notes);
    Task<AppointmentDto?> UpdateAppointmentStatusAsync(int appointmentId, string status);
    Task<bool> DeleteAppointmentAsync(int appointmentId);
}


