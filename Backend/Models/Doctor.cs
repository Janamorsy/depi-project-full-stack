using System.Text.Json.Serialization;

namespace NileCareAPI.Models;


public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string SpecialtyTags { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public double Rating { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<AvailabilitySlot> AvailabilitySlots { get; set; }
           = new List<AvailabilitySlot>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public string NationalIdFrontImageUrl { get; set; } = string.Empty;
    public string NationalIdBackImageUrl { get; set; } = string.Empty;


}

