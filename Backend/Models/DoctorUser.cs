using Microsoft.AspNetCore.Identity;

namespace NileCareAPI.Models;

public class DoctorUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string SpecialtyTags { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = "/images/default-avatar.png";
    public bool IsAvailable { get; set; } = true;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
  
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    public IEnumerable<AvailabilitySlot> AvailabilitySlots { get;  set; }
    public string NationalIdFrontImageUrl { get; set; } = string.Empty;
    public string NationalIdBackImageUrl { get; set; } = string.Empty;
}



