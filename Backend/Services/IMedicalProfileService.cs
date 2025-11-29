using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IMedicalProfileService
{
    Task<MedicalProfileDto?> GetProfileAsync();
    Task<MedicalProfileDto> CreateOrUpdateProfileAsync(MedicalProfileDto profileDto);
}


