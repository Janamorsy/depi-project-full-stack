using System;

namespace NileCareAPI.Models
{
    public class HotelBooking
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public decimal RoomRatePerNight { get; set; }
        public int NumberOfNights => (CheckOutDate - CheckInDate).Days > 0 ? (CheckOutDate - CheckInDate).Days : 1;

        public decimal TotalPrice => NumberOfNights * RoomRatePerNight;
        public string Status { get; set; } = "Pending";
        public string SpecialRequests { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PaymentStatus { get; set; } = "Unpaid"; // Recommended default value
      }
    }