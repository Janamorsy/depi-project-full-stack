using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IDoctorRepository
{
    Task<Doctor?> GetByIdAsync(int id);
    Task<List<Doctor>> SearchAsync(string? city, string? specialty, string? language);
}