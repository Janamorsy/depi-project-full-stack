using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class DashboardService : IDashboardService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMedicalProfileRepository _medicalProfileRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public DashboardService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        IMedicalProfileRepository medicalProfileRepository,
        IAppointmentRepository appointmentRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _medicalProfileRepository = medicalProfileRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedAccessException();

        var medicalProfile = await _medicalProfileRepository.GetByUserIdAsync(userId);
        var appointments = await _appointmentRepository.GetByUserIdAsync(userId);

        var upcomingAppointments = appointments
            .Where(a => a.AppointmentDate >= DateTime.UtcNow)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = userId,
                DoctorId = a.DoctorUserId,
                DoctorName = a.DoctorUser != null ? $"Dr. {a.DoctorUser.FirstName} {a.DoctorUser.LastName}" : (a.LegacyDoctor != null ? a.LegacyDoctor.Name : "Unknown"),
                DoctorPhoneNumber = a.DoctorUser?.PhoneNumber ?? "",
                Specialty = a.DoctorUser?.Specialty ?? a.LegacyDoctor?.Specialty ?? "Unknown",
                Hospital = a.DoctorUser?.Hospital ?? a.LegacyDoctor?.Hospital ?? "Unknown",
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                PatientNotes = a.PatientNotes,
                DoctorNotes = a.DoctorNotes,
                AppointmentType = a.AppointmentType,
                ConsultationFee = a.ConsultationFee,
                PaymentStatus = a.PaymentStatus
            })
            .ToList();

        var timelineEvents = upcomingAppointments.Select(a => new TimelineEventDto
        {
            Type = "Appointment",
            Title = $"Consultation with {a.DoctorName}",
            Description = $"{a.Specialty} at {a.Hospital}",
            Date = a.AppointmentDate,
            Status = a.Status
        }).ToList();

        return new DashboardDto
        {
            User = new UserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                ProfilePicture = user.ProfilePicture
            },
            MedicalProfile = medicalProfile == null ? null : new MedicalProfileDto
            {
                Id = medicalProfile.Id,
                MedicalConditions = medicalProfile.MedicalConditions,
                AccessibilityNeeds = medicalProfile.AccessibilityNeeds,
                TreatmentHistory = medicalProfile.TreatmentHistory,
                ConsentGiven = medicalProfile.ConsentGiven
            },
            UpcomingEvents = timelineEvents,
            UpcomingAppointments = upcomingAppointments
        };
    }
}


