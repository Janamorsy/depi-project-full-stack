namespace NileCareAPI.DTOs;



public class UpdateProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePicture { get; set; }
    public bool? IsWheelchairAccessible { get; set; }
}

