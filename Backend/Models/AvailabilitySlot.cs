using NileCareAPI.Models;

public class AvailabilitySlot
{
    public int Id { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }

    public string? DoctorUserId { get; set; }
    public DoctorUser? DoctorUser { get; set; }

    public int? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
