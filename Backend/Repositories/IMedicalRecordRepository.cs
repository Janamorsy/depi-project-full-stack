using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IMedicalRecordRepository
{
    Task<List<MedicalRecord>> GetByUserIdAsync(string userId);
    Task<MedicalRecord?> GetByIdAsync(int id);
    Task<MedicalRecord> CreateAsync(MedicalRecord record);
    Task DeleteAsync(int id);
}