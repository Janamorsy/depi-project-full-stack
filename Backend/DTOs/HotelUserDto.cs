namespace NileCareAPI.DTOs;

public class HotelUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public class HotelUserRegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class HotelRequestDto
{
    // Basic hotel info
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Amenities { get; set; } = string.Empty;
    
    // Accessibility features
    public bool WheelchairAccessible { get; set; }
    public bool RollInShower { get; set; }
    public bool ElevatorAccess { get; set; }
    public bool GrabBars { get; set; }
    
    // Room pricing
    public decimal StandardRoomPrice { get; set; }
    public int StandardRoomMaxGuests { get; set; } = 2;
    public decimal DeluxeRoomPrice { get; set; }
    public int DeluxeRoomMaxGuests { get; set; } = 3;
    public decimal SuiteRoomPrice { get; set; }
    public int SuiteRoomMaxGuests { get; set; } = 4;
    public decimal FamilyRoomPrice { get; set; }
    public int FamilyRoomMaxGuests { get; set; } = 6;
}

public class PendingHotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Amenities { get; set; } = string.Empty;
    public decimal StandardRoomPrice { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool RollInShower { get; set; }
    public bool ElevatorAccess { get; set; }
    public bool GrabBars { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Owner info
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerCompany { get; set; } = string.Empty;
    
    // Images
    public List<HotelImageDto> Images { get; set; } = new();
}

public class HotelRejectDto
{
    public string Reason { get; set; } = string.Empty;
}
