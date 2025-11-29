namespace NileCareAPI.DTOs;

public class DashboardDto
{
    public UserInfoDto User { get; set; } = new();
    public MedicalProfileDto? MedicalProfile { get; set; }
    public List<TimelineEventDto> UpcomingEvents { get; set; } = new();
    public List<AppointmentDto> UpcomingAppointments { get; set; } = new();
}

public class UserInfoDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
}


