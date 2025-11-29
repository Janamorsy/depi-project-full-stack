using System.Text.Json.Serialization;

namespace NileCareAPI.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; } // Base price (Standard room)
    public double Rating { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool RollInShower { get; set; }
    public bool ElevatorAccess { get; set; }
    public bool GrabBars { get; set; }
    public string Amenities { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty; // Primary/legacy image
    public string Description { get; set; } = string.Empty;
    
    // Room type pricing
    public decimal StandardRoomPrice { get; set; }
    public int StandardRoomMaxGuests { get; set; } = 2;
    public decimal DeluxeRoomPrice { get; set; }
    public int DeluxeRoomMaxGuests { get; set; } = 3;
    public decimal SuiteRoomPrice { get; set; }
    public int SuiteRoomMaxGuests { get; set; } = 4;
    public decimal FamilyRoomPrice { get; set; }
    public int FamilyRoomMaxGuests { get; set; } = 6;
    
    // Approval workflow
    public string ApprovalStatus { get; set; } = "Approved"; // Pending, Approved, Rejected
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Hotel Owner relationship
    public string? OwnerId { get; set; }
    [JsonIgnore]
    public HotelUser? Owner { get; set; }
    
    // Multiple images collection
    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
}


