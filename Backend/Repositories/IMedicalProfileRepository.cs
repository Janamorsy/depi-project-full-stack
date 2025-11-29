using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IMedicalProfileRepository
{
    Task<MedicalProfile?> GetByUserIdAsync(string userId);
    Task<MedicalProfile> CreateAsync(MedicalProfile profile);
    Task<MedicalProfile> UpdateAsync(MedicalProfile profile);
}