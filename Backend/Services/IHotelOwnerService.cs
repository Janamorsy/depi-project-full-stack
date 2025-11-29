using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public interface IHotelOwnerService
{
    Task<Hotel> SubmitHotelRequestAsync(string ownerId, HotelRequestDto dto, List<IFormFile>? images);
    Task<List<Hotel>> GetMyHotelsAsync(string ownerId);
    Task<Hotel?> GetHotelByIdAsync(string ownerId, int hotelId);
    Task<Hotel?> UpdateHotelAsync(string ownerId, int hotelId, HotelRequestDto dto, List<IFormFile>? newImages, List<int>? deleteImageIds);
    Task<bool> DeleteHotelAsync(string ownerId, int hotelId);
    Task<HotelDashboardStatsDto> GetDashboardStatsAsync(string ownerId);
    Task<List<HotelBookingDetailDto>> GetHotelBookingsAsync(string ownerId, int? hotelId = null);
    Task<HotelBookingDetailDto?> UpdateBookingStatusAsync(string ownerId, int bookingId, string status);
    Task<List<HotelBookingCountDto>> GetBookingCountsPerHotelAsync(string ownerId);
}

public class HotelDashboardStatsDto
{
    public int TotalHotels { get; set; }
    public int ApprovedHotels { get; set; }
    public int PendingHotels { get; set; }
    public int RejectedHotels { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int RejectedBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class HotelBookingDetailDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
    public string GuestAvatar { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string SpecialRequests { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class HotelBookingCountDto
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int RejectedBookings { get; set; }
}
