using Microsoft.AspNetCore.Identity;

namespace NileCareAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = "/images/default-avatar.png";
    public bool IsWheelchairAccessible { get; set; } = false;
    public MedicalProfile? MedicalProfile { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<HotelBooking> HotelBookings { get; set; } = new List<HotelBooking>();
    public ICollection<TransportBooking> TransportBookings { get; set; } = new List<TransportBooking>();
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}


