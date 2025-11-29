using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IDoctorUserRepository
{
    Task<DoctorUser?> GetByIdAsync(string id);
    Task<List<DoctorUser>> SearchAsync(string? city, string? specialty, string? language);
}


