namespace NileCareAPI.Models;

public class MedicalProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string MedicalConditions { get; set; } = string.Empty;
    public string AccessibilityNeeds { get; set; } = string.Empty;
    public string TreatmentHistory { get; set; } = string.Empty;
    public bool ConsentGiven { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}


