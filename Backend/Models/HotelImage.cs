using System.Text.Json.Serialization;

namespace NileCareAPI.Models;

public class HotelImage
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    [JsonIgnore]
    public Hotel Hotel { get; set; } = null!;
}
