namespace NileCareAPI.DTOs;

public class DoctorDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NationalIdFrontImageUrl { get; set; } = string.Empty;
    public string NationalIdBackImageUrl { get; set; } = string.Empty;
}


