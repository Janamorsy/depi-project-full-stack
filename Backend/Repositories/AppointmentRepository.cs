using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly NileCareDbContextV2 _context;

    public AppointmentRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetByUserIdAsync(string userId)
    {
        return await _context.Appointments
            .Include(a => a.DoctorUser)
            .Include(a => a.LegacyDoctor)
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.DoctorUser)
            .Include(a => a.LegacyDoctor)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasAppointmentWithPatientAsync(string doctorId, string patientId)
    {
        return await _context.Appointments
            .AnyAsync(a => a.DoctorUserId == doctorId && a.UserId == patientId);
    }

    public async Task<int> GetQueueCountForSlotAsync(string? doctorUserId, int? legacyDoctorId, DateTime slotStart, DateTime slotEnd)
    {
        return await _context.Appointments
            .Where(a => 
                (doctorUserId != null && a.DoctorUserId == doctorUserId) ||
                (legacyDoctorId != null && a.LegacyDoctorId == legacyDoctorId))
            .Where(a => a.AppointmentDate >= slotStart && a.AppointmentDate < slotEnd)
            .CountAsync();
    }

    public async Task<int> GetTotalInSlotAsync(string? doctorUserId, int? legacyDoctorId, DateTime slotStart, DateTime slotEnd)
    {
        return await _context.Appointments
            .Where(a => 
                (doctorUserId != null && a.DoctorUserId == doctorUserId) ||
                (legacyDoctorId != null && a.LegacyDoctorId == legacyDoctorId))
            .Where(a => a.AppointmentDate >= slotStart && a.AppointmentDate < slotEnd)
            .CountAsync();
    }
}


