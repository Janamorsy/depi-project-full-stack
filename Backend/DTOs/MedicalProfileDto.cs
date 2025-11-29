namespace NileCareAPI.DTOs;

public class MedicalProfileDto
{
    public int Id { get; set; }
    public string MedicalConditions { get; set; } = string.Empty;
    public string AccessibilityNeeds { get; set; } = string.Empty;
    public string TreatmentHistory { get; set; } = string.Empty;
    public bool ConsentGiven { get; set; }
}


