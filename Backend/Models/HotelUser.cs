using Microsoft.AspNetCore.Identity;

namespace NileCareAPI.Models;

public class HotelUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = "/images/default-avatar.png";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation property for owned hotels
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
