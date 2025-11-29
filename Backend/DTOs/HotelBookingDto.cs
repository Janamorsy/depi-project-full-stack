namespace NileCareAPI.DTOs;

public class HotelBookingDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string SpecialRequests { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = "Unpaid";
}

public class CreateHotelBookingDto
{
    public int HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string RoomType { get; set; } = "Standard";
    public string SpecialRequests { get; set; } = string.Empty;
}

public class RoomTypeDto
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
}

public class HotelWithRoomTypesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; } // Base price
    public double Rating { get; set; }
    public bool WheelchairAccessible { get; set; }
    public bool RollInShower { get; set; }
    public bool ElevatorAccess { get; set; }
    public bool GrabBars { get; set; }
    public string Amenities { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RoomTypeDto> RoomTypes { get; set; } = new();
    public List<HotelImageDto> Images { get; set; } = new();
}

public class HotelImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}


