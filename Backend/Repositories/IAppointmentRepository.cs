using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetByUserIdAsync(string userId);
    Task<Appointment?> GetByIdAsync(int id);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task<Appointment> UpdateAsync(Appointment appointment);
    Task DeleteAsync(int id);
    Task<bool> HasAppointmentWithPatientAsync(string doctorId, string patientId);
    Task<int> GetQueueCountForSlotAsync(string? doctorUserId, int? legacyDoctorId, DateTime slotStart, DateTime slotEnd);
    Task<int> GetTotalInSlotAsync(string? doctorUserId, int? legacyDoctorId, DateTime slotStart, DateTime slotEnd);
}


