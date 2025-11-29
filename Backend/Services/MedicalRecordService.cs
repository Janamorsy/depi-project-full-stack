using System.Security.Claims;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IWebHostEnvironment _environment;

    public MedicalRecordService(
        IHttpContextAccessor httpContextAccessor,
        IMedicalRecordRepository medicalRecordRepository,
        IAppointmentRepository appointmentRepository,
        IWebHostEnvironment environment)
    {
        _httpContextAccessor = httpContextAccessor;
        _medicalRecordRepository = medicalRecordRepository;
        _appointmentRepository = appointmentRepository;
        _environment = environment;
    }

    private string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException();
    }

    private bool IsDoctor()
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole("Doctor") ?? false;
    }

    public async Task<MedicalRecordDto> UploadRecordAsync(IFormFile file, UploadMedicalRecordDto uploadDto)
    {
        var userId = GetUserId();

        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("File type not allowed");

        if (file.Length > 10 * 1024 * 1024)
            throw new ArgumentException("File size exceeds 10MB limit");

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "medical-records");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        var record = new MedicalRecord
        {
            UserId = userId,
            FileName = file.FileName,
            FileType = fileExtension,
            FileUrl = $"/uploads/medical-records/{uniqueFileName}",
            FileSize = file.Length,
            Description = uploadDto.Description,
            Category = uploadDto.Category
        };

        await _medicalRecordRepository.CreateAsync(record);

        return new MedicalRecordDto
        {
            Id = record.Id,
            FileName = record.FileName,
            FileType = record.FileType,
            FileUrl = record.FileUrl,
            FileSize = record.FileSize,
            Description = record.Description,
            Category = record.Category,
            UploadedAt = record.UploadedAt
        };
    }

    public async Task<List<MedicalRecordDto>> GetUserRecordsAsync()
    {
        var userId = GetUserId();
        var records = await _medicalRecordRepository.GetByUserIdAsync(userId);

        return records.Select(r => new MedicalRecordDto
        {
            Id = r.Id,
            FileName = r.FileName,
            FileType = r.FileType,
            FileUrl = r.FileUrl,
            FileSize = r.FileSize,
            Description = r.Description,
            Category = r.Category,
            UploadedAt = r.UploadedAt
        }).ToList();
    }

    public async Task<List<MedicalRecordDto>> GetPatientRecordsAsync(string patientId)
    {
        if (!IsDoctor())
            throw new UnauthorizedAccessException("Only doctors can view patient records");

        var doctorId = GetUserId();
        
        // Security: Verify doctor has an appointment with this patient
        var hasAppointment = await _appointmentRepository.HasAppointmentWithPatientAsync(doctorId, patientId);
        if (!hasAppointment)
            throw new UnauthorizedAccessException("You can only view records of patients you have appointments with.");

        var records = await _medicalRecordRepository.GetByUserIdAsync(patientId);

        return records.Select(r => new MedicalRecordDto
        {
            Id = r.Id,
            FileName = r.FileName,
            FileType = r.FileType,
            FileUrl = r.FileUrl,
            FileSize = r.FileSize,
            Description = r.Description,
            Category = r.Category,
            UploadedAt = r.UploadedAt
        }).ToList();
    }

    public async Task<MedicalRecordDto?> GetRecordAsync(int id)
    {
        var record = await _medicalRecordRepository.GetByIdAsync(id);
        if (record == null)
            return null;

        var userId = GetUserId();
        if (record.UserId != userId && !IsDoctor())
            throw new UnauthorizedAccessException();

        return new MedicalRecordDto
        {
            Id = record.Id,
            FileName = record.FileName,
            FileType = record.FileType,
            FileUrl = record.FileUrl,
            FileSize = record.FileSize,
            Description = record.Description,
            Category = record.Category,
            UploadedAt = record.UploadedAt
        };
    }

    public async Task DeleteRecordAsync(int id)
    {
        var record = await _medicalRecordRepository.GetByIdAsync(id);
        if (record == null)
            return;

        var userId = GetUserId();
        if (record.UserId != userId)
            throw new UnauthorizedAccessException();

        var filePath = Path.Combine(_environment.WebRootPath, record.FileUrl.TrimStart('/'));
        if (File.Exists(filePath))
            File.Delete(filePath);

        await _medicalRecordRepository.DeleteAsync(id);
    }
}


