namespace NileCareAPI.DTOs;

public class DoctorSearchResultDto
{
    public int Id { get; set; }
    public string? DoctorUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public double Rating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsRecommended { get; set; }

    public List<DoctorAvailabilitySlotDto> AvailabilitySlots { get; set; } = new();

}


