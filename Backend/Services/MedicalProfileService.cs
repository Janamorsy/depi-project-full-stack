using System.Security.Claims;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class MedicalProfileService : IMedicalProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMedicalProfileRepository _medicalProfileRepository;

    public MedicalProfileService(
        IHttpContextAccessor httpContextAccessor,
        IMedicalProfileRepository medicalProfileRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _medicalProfileRepository = medicalProfileRepository;
    }

    public async Task<MedicalProfileDto?> GetProfileAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException();

        var profile = await _medicalProfileRepository.GetByUserIdAsync(userId);
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

    public async Task<MedicalProfileDto> CreateOrUpdateProfileAsync(MedicalProfileDto profileDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException();

        var existingProfile = await _medicalProfileRepository.GetByUserIdAsync(userId);

        if (existingProfile == null)
        {
            var newProfile = new MedicalProfile
            {
                UserId = userId,
                MedicalConditions = profileDto.MedicalConditions,
                AccessibilityNeeds = profileDto.AccessibilityNeeds,
                TreatmentHistory = profileDto.TreatmentHistory,
                ConsentGiven = profileDto.ConsentGiven
            };

            var created = await _medicalProfileRepository.CreateAsync(newProfile);
            profileDto.Id = created.Id;
        }
        else
        {
            existingProfile.MedicalConditions = profileDto.MedicalConditions;
            existingProfile.AccessibilityNeeds = profileDto.AccessibilityNeeds;
            existingProfile.TreatmentHistory = profileDto.TreatmentHistory;
            existingProfile.ConsentGiven = profileDto.ConsentGiven;

            await _medicalProfileRepository.UpdateAsync(existingProfile);
        }

        return profileDto;
    }
}


