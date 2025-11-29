namespace NileCareAPI.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsWheelchairAccessible { get; set; }
    public string Token { get; set; } = string.Empty;
}


