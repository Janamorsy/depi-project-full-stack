using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IMedicalRecordService
{
    Task<MedicalRecordDto> UploadRecordAsync(IFormFile file, UploadMedicalRecordDto uploadDto);
    Task<List<MedicalRecordDto>> GetUserRecordsAsync();
    Task<List<MedicalRecordDto>> GetPatientRecordsAsync(string patientId);
    Task<MedicalRecordDto?> GetRecordAsync(int id);
    Task DeleteRecordAsync(int id);
}


