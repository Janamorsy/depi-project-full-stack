namespace NileCareAPI.DTOs;

public class AdminLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AdminRegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AdminSecret { get; set; } // Secret key to allow admin registration
}

public class AdminDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

// DTOs for managing users
public class UserListDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string ProfilePicture { get; set; } = string.Empty;
    public bool IsWheelchairAccessible { get; set; }
    public int AppointmentsCount { get; set; }
    public int HotelBookingsCount { get; set; }
    public int TransportBookingsCount { get; set; }
}

// DTOs for managing hotels
public class CreateHotelDto
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public double Rating { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool RollInShower { get; set; }
    public bool ElevatorAccess { get; set; }
    public bool GrabBars { get; set; }
    public string Amenities { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal StandardRoomPrice { get; set; }
    public int StandardRoomMaxGuests { get; set; } = 2;
    public decimal DeluxeRoomPrice { get; set; }
    public int DeluxeRoomMaxGuests { get; set; } = 3;
    public decimal SuiteRoomPrice { get; set; }
    public int SuiteRoomMaxGuests { get; set; } = 4;
    public decimal FamilyRoomPrice { get; set; }
    public int FamilyRoomMaxGuests { get; set; } = 6;
}

public class UpdateHotelDto : CreateHotelDto
{
    public int Id { get; set; }
}

// DTOs for managing transport
public class CreateTransportDto
{
    public string VehicleType { get; set; } = string.Empty;
    public bool WheelchairAccessible { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerHour { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class UpdateTransportDto : CreateTransportDto
{
    public int Id { get; set; }
}

// Dashboard stats
public class AdminDashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalHotels { get; set; }
    public int TotalTransports { get; set; }
    public int TotalAppointments { get; set; }
    public int TotalHotelBookings { get; set; }
    public int TotalTransportBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingHotelRequests { get; set; }
}
