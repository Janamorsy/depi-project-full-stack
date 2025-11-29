using System.Security.Claims;
using NileCareAPI.DTOs;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class DoctorService : IDoctorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IDoctorUserRepository _doctorUserRepository;
    private readonly IMedicalProfileRepository _medicalProfileRepository;

    public DoctorService(
        IHttpContextAccessor httpContextAccessor,
        IDoctorRepository doctorRepository,
        IDoctorUserRepository doctorUserRepository,
        IMedicalProfileRepository medicalProfileRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _doctorRepository = doctorRepository;
        _doctorUserRepository = doctorUserRepository;
        _medicalProfileRepository = medicalProfileRepository;
    }

    public async Task<List<DoctorSearchResultDto>> SearchDoctorsAsync(string? city, string? specialty, string? language)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var doctorUsers = await _doctorUserRepository.SearchAsync(city, specialty, language);
        var legacyDoctors = await _doctorRepository.SearchAsync(city, specialty, language);

        string? userCondition = null;
        if (!string.IsNullOrEmpty(userId))
        {
            var profile = await _medicalProfileRepository.GetByUserIdAsync(userId);
            userCondition = profile?.MedicalConditions;
        }

        var results = new List<DoctorSearchResultDto>();

        foreach (var du in doctorUsers)
        {
            results.Add(new DoctorSearchResultDto
            {
                Id = 0,
                DoctorUserId = du.Id,
                Name = $"Dr. {du.FirstName} {du.LastName}",
                Specialty = du.Specialty,
                Hospital = du.Hospital,
                City = du.City,
                Languages = du.Languages,
                ConsultationFee = du.ConsultationFee,
                YearsOfExperience = du.YearsOfExperience,
                Rating = 4.5,
                ImageUrl = du.ImageUrl,
                IsRecommended = !string.IsNullOrEmpty(userCondition) &&
                               du.SpecialtyTags.Contains(userCondition, StringComparison.OrdinalIgnoreCase),
                AvailabilitySlots = du.AvailabilitySlots.Select(s => new DoctorAvailabilitySlotDto
                {
                    Day = (int)s.Day,
                    Start = s.Start.ToString(@"hh\:mm"),
                    End = s.End.ToString(@"hh\:mm")
                }).ToList()
            });
        }

        foreach (var d in legacyDoctors)
        {
            results.Add(new DoctorSearchResultDto
            {
                Id = d.Id,
                DoctorUserId = null,
                Name = d.Name,
                Specialty = d.Specialty,
                Hospital = d.Hospital,
                City = d.City,
                Languages = d.Languages,
                ConsultationFee = d.ConsultationFee,
                YearsOfExperience = d.YearsOfExperience,
                Rating = d.Rating,
                ImageUrl = d.ImageUrl,
                IsRecommended = !string.IsNullOrEmpty(userCondition) &&
                               d.SpecialtyTags.Contains(userCondition, StringComparison.OrdinalIgnoreCase),
                AvailabilitySlots = d.AvailabilitySlots.Select(s => new DoctorAvailabilitySlotDto
                {
                    Day = (int)s.Day,
                    Start = s.Start.ToString(@"hh\:mm"),
                    End = s.End.ToString(@"hh\:mm")
                }).ToList()
            });
        }

        return results.OrderByDescending(d => d.IsRecommended)
                      .ThenByDescending(d => d.Rating)
                      .ToList();
    }

}


