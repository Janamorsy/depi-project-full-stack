using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.DTOs;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class DoctorDashboardService : IDoctorDashboardService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NileCareDbContextV2 _context;
    private readonly IMedicalProfileRepository _medicalProfileRepository;

    public DoctorDashboardService(
        IHttpContextAccessor httpContextAccessor,
        NileCareDbContextV2 context,
        IMedicalProfileRepository medicalProfileRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _medicalProfileRepository = medicalProfileRepository;
    }

    private string GetDoctorId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException();
    }

    public async Task<List<AppointmentDto>> GetDoctorAppointmentsAsync()
    {
        var doctorId = GetDoctorId();

        var appointments = await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.DoctorUser)
            .Where(a => a.DoctorUserId == doctorId)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        var appointmentDtos = new List<AppointmentDto>();
        foreach (var a in appointments)
        {
            // Calculate slot boundaries (same hour on the same day)
            var slotStart = new DateTime(a.AppointmentDate.Year, a.AppointmentDate.Month, a.AppointmentDate.Day, a.AppointmentDate.Hour, 0, 0);
            var slotEnd = slotStart.AddHours(1);

            var totalInSlot = await _context.Appointments
                .Where(apt => apt.DoctorUserId == doctorId)
                .Where(apt => apt.AppointmentDate >= slotStart && apt.AppointmentDate < slotEnd)
                .CountAsync();

            appointmentDtos.Add(new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.UserId,
                PatientName = $"{a.User.FirstName} {a.User.LastName}",
                PatientPhoneNumber = a.User.PhoneNumber ?? "",
                DoctorId = doctorId,
                DoctorName = a.DoctorUser != null ? $"Dr. {a.DoctorUser.FirstName} {a.DoctorUser.LastName}" : "",
                Specialty = a.DoctorUser?.Specialty ?? "",
                Hospital = a.DoctorUser?.Hospital ?? "",
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                PatientNotes = a.PatientNotes,
                DoctorNotes = a.DoctorNotes,
                AppointmentType = a.AppointmentType,
                ConsultationFee = a.ConsultationFee,
                QueueNumber = a.QueueNumber,
                TotalInSlot = totalInSlot
            });
        }

        return appointmentDtos;
    }

    public async Task<MedicalProfileDto?> GetPatientProfileAsync(string patientId)
    {
        var doctorId = GetDoctorId();
        
        // Security: Verify doctor has an appointment with this patient
        var hasAppointment = await _context.Appointments
            .AnyAsync(a => a.DoctorUserId == doctorId && a.UserId == patientId);
        
        if (!hasAppointment)
            throw new UnauthorizedAccessException("You can only view profiles of patients you have appointments with.");

        var profile = await _medicalProfileRepository.GetByUserIdAsync(patientId);
        if (profile == null)
            return null;

        return new MedicalProfileDto
        {
            Id = profile.Id,
            MedicalConditions = profile.MedicalConditions,
            AccessibilityNeeds = profile.AccessibilityNeeds,
            TreatmentHistory = profile.TreatmentHistory,
            ConsentGiven = profile.ConsentGiven
        };
    }

    public async Task UpdateAppointmentNotesAsync(int appointmentId, string notes)
    {
        var doctorId = GetDoctorId();
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.DoctorUserId == doctorId);

        if (appointment == null)
            throw new ArgumentException("Appointment not found");

        appointment.DoctorNotes = notes;
        appointment.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentDto?> UpdateAppointmentStatusAsync(int appointmentId, string status)
    {
        var doctorId = GetDoctorId();
        var appointment = await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.DoctorUser)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.DoctorUserId == doctorId);

        if (appointment == null)
            return null;

        appointment.Status = status;
        appointment.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.UserId,
            PatientName = $"{appointment.User.FirstName} {appointment.User.LastName}",
            PatientPhoneNumber = appointment.User.PhoneNumber ?? "",
            DoctorId = doctorId,
            DoctorName = appointment.DoctorUser != null ? $"Dr. {appointment.DoctorUser.FirstName} {appointment.DoctorUser.LastName}" : "",
            Specialty = appointment.DoctorUser?.Specialty ?? "",
            Hospital = appointment.DoctorUser?.Hospital ?? "",
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            PatientNotes = appointment.PatientNotes,
            DoctorNotes = appointment.DoctorNotes,
            AppointmentType = appointment.AppointmentType,
            ConsultationFee = appointment.ConsultationFee
        };
    }

    public async Task<bool> DeleteAppointmentAsync(int appointmentId)
    {
        var doctorId = GetDoctorId();
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.DoctorUserId == doctorId);

        if (appointment == null)
            return false;

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }
}


