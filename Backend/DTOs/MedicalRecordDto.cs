namespace NileCareAPI.DTOs;

public class MedicalRecordDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}

public class UploadMedicalRecordDto
{
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}


