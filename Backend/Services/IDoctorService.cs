using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IDoctorService
{
    Task<List<DoctorSearchResultDto>> SearchDoctorsAsync(string? city, string? specialty, string? language);
}


