namespace NileCareAPI.DTOs
{
    public class DoctorAvailabilitySlotDto
    {
        public int Day { get; set; }       
        public string Start { get; set; } = string.Empty; 
        public string End { get; set; } = string.Empty;  
    }
}