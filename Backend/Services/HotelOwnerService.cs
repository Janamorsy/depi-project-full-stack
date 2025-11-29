using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public class HotelOwnerService : IHotelOwnerService
{
    private readonly NileCareDbContextV2 _context;
    private readonly IWebHostEnvironment _environment;

    public HotelOwnerService(NileCareDbContextV2 context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<Hotel> SubmitHotelRequestAsync(string ownerId, HotelRequestDto dto, List<IFormFile>? images)
    {
        var hotel = new Hotel
        {
            Name = dto.Name,
            City = dto.City,
            Address = dto.Address,
            Description = dto.Description,
            Amenities = dto.Amenities,
            WheelchairAccessible = dto.WheelchairAccessible,
            RollInShower = dto.RollInShower,
            ElevatorAccess = dto.ElevatorAccess,
            GrabBars = dto.GrabBars,
            StandardRoomPrice = dto.StandardRoomPrice,
            StandardRoomMaxGuests = dto.StandardRoomMaxGuests,
            DeluxeRoomPrice = dto.DeluxeRoomPrice,
            DeluxeRoomMaxGuests = dto.DeluxeRoomMaxGuests,
            SuiteRoomPrice = dto.SuiteRoomPrice,
            SuiteRoomMaxGuests = dto.SuiteRoomMaxGuests,
            FamilyRoomPrice = dto.FamilyRoomPrice,
            FamilyRoomMaxGuests = dto.FamilyRoomMaxGuests,
            PricePerNight = dto.StandardRoomPrice, // Base price is standard room
            Rating = 0, // New hotels start with no rating
            ApprovalStatus = "Pending",
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        // Handle images
        if (images != null && images.Count > 0)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "hotels");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            bool isFirst = true;
            foreach (var image in images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var imageUrl = $"/uploads/hotels/{fileName}";

                // Set first image as primary
                if (isFirst)
                {
                    hotel.ImageUrl = imageUrl;
                    isFirst = false;
                }

                var hotelImage = new HotelImage
                {
                    HotelId = hotel.Id,
                    ImageUrl = imageUrl,
                    DisplayOrder = hotel.Images.Count
                };
                _context.HotelImages.Add(hotelImage);
            }

            await _context.SaveChangesAsync();
        }

        return await _context.Hotels
            .Include(h => h.Images)
            .FirstAsync(h => h.Id == hotel.Id);
    }

    public async Task<List<Hotel>> GetMyHotelsAsync(string ownerId)
    {
        return await _context.Hotels
            .Include(h => h.Images)
            .Where(h => h.OwnerId == ownerId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<Hotel?> GetHotelByIdAsync(string ownerId, int hotelId)
    {
        return await _context.Hotels
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == hotelId && h.OwnerId == ownerId);
    }

    public async Task<Hotel?> UpdateHotelAsync(string ownerId, int hotelId, HotelRequestDto dto, List<IFormFile>? newImages, List<int>? deleteImageIds)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == hotelId && h.OwnerId == ownerId);

        if (hotel == null)
            return null;

        // Only allow edits if pending or rejected (resubmission)
        if (hotel.ApprovalStatus == "Approved")
        {
            // For approved hotels, only allow certain updates
            hotel.Description = dto.Description;
            hotel.Amenities = dto.Amenities;
        }
        else
        {
            hotel.Name = dto.Name;
            hotel.City = dto.City;
            hotel.Address = dto.Address;
            hotel.Description = dto.Description;
            hotel.Amenities = dto.Amenities;
            hotel.WheelchairAccessible = dto.WheelchairAccessible;
            hotel.RollInShower = dto.RollInShower;
            hotel.ElevatorAccess = dto.ElevatorAccess;
            hotel.GrabBars = dto.GrabBars;
            hotel.StandardRoomPrice = dto.StandardRoomPrice;
            hotel.StandardRoomMaxGuests = dto.StandardRoomMaxGuests;
            hotel.DeluxeRoomPrice = dto.DeluxeRoomPrice;
            hotel.DeluxeRoomMaxGuests = dto.DeluxeRoomMaxGuests;
            hotel.SuiteRoomPrice = dto.SuiteRoomPrice;
            hotel.SuiteRoomMaxGuests = dto.SuiteRoomMaxGuests;
            hotel.FamilyRoomPrice = dto.FamilyRoomPrice;
            hotel.FamilyRoomMaxGuests = dto.FamilyRoomMaxGuests;
            hotel.PricePerNight = dto.StandardRoomPrice;

            // If rejected, set back to pending for re-review
            if (hotel.ApprovalStatus == "Rejected")
            {
                hotel.ApprovalStatus = "Pending";
                hotel.RejectionReason = null;
            }
        }

        // Delete specified images
        if (deleteImageIds != null && deleteImageIds.Count > 0)
        {
            var imagesToDelete = hotel.Images.Where(i => deleteImageIds.Contains(i.Id)).ToList();
            foreach (var image in imagesToDelete)
            {
                var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                
                _context.HotelImages.Remove(image);
            }
        }

        // Add new images
        if (newImages != null && newImages.Count > 0)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "hotels");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var image in newImages)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var imageUrl = $"/uploads/hotels/{fileName}";

                // Set as primary if no primary image
                if (string.IsNullOrEmpty(hotel.ImageUrl))
                    hotel.ImageUrl = imageUrl;

                var hotelImage = new HotelImage
                {
                    HotelId = hotel.Id,
                    ImageUrl = imageUrl,
                    DisplayOrder = hotel.Images.Count
                };
                _context.HotelImages.Add(hotelImage);
            }
        }

        await _context.SaveChangesAsync();
        return hotel;
    }

    public async Task<bool> DeleteHotelAsync(string ownerId, int hotelId)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == hotelId && h.OwnerId == ownerId);

        if (hotel == null)
            return false;

        // Only allow deletion of pending or rejected hotels
        if (hotel.ApprovalStatus == "Approved")
            return false;

        // Delete images
        foreach (var image in hotel.Images)
        {
            var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<HotelDashboardStatsDto> GetDashboardStatsAsync(string ownerId)
    {
        var hotels = await _context.Hotels
            .Where(h => h.OwnerId == ownerId)
            .ToListAsync();

        var hotelIds = hotels.Select(h => h.Id).ToList();

        var bookings = await _context.HotelBookings
            .Where(b => hotelIds.Contains(b.HotelId))
            .ToListAsync();

        return new HotelDashboardStatsDto
        {
            TotalHotels = hotels.Count,
            ApprovedHotels = hotels.Count(h => h.ApprovalStatus == "Approved"),
            PendingHotels = hotels.Count(h => h.ApprovalStatus == "Pending"),
            RejectedHotels = hotels.Count(h => h.ApprovalStatus == "Rejected"),
            TotalBookings = bookings.Count,
            PendingBookings = bookings.Count(b => b.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) || b.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)),
            ConfirmedBookings = bookings.Count(b => b.Status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase)),
            RejectedBookings = bookings.Count(b => b.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase) || b.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)),
            TotalRevenue = bookings.Where(b => b.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase)).Sum(b => b.RoomRatePerNight * (decimal)(b.CheckOutDate - b.CheckInDate).TotalDays)
        };
    }

    public async Task<List<HotelBookingDetailDto>> GetHotelBookingsAsync(string ownerId, int? hotelId = null)
    {
        var hotelIds = await _context.Hotels
            .Where(h => h.OwnerId == ownerId)
            .Select(h => h.Id)
            .ToListAsync();

        var query = _context.HotelBookings
            .Include(b => b.Hotel)
            .Include(b => b.User)
            .Where(b => hotelIds.Contains(b.HotelId));

        if (hotelId.HasValue)
        {
            query = query.Where(b => b.HotelId == hotelId.Value);
        }

        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => new HotelBookingDetailDto
        {
            Id = b.Id,
            HotelId = b.HotelId,
            HotelName = b.Hotel.Name,
            GuestName = $"{b.User.FirstName} {b.User.LastName}",
            GuestEmail = b.User.Email ?? string.Empty,
            GuestPhone = b.User.PhoneNumber ?? string.Empty,
            GuestAvatar = b.User.ProfilePicture ?? "/images/default-avatar.png",
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            NumberOfGuests = b.NumberOfGuests,
            RoomType = b.RoomType,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            PaymentStatus = b.PaymentStatus,
            SpecialRequests = b.SpecialRequests,
            CreatedAt = b.CreatedAt
        }).ToList();
    }

    public async Task<HotelBookingDetailDto?> UpdateBookingStatusAsync(string ownerId, int bookingId, string status)
    {
        var hotelIds = await _context.Hotels
            .Where(h => h.OwnerId == ownerId)
            .Select(h => h.Id)
            .ToListAsync();

        var booking = await _context.HotelBookings
            .Include(b => b.Hotel)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == bookingId && hotelIds.Contains(b.HotelId));

        if (booking == null)
            return null;

        booking.Status = status;
        await _context.SaveChangesAsync();

        return new HotelBookingDetailDto
        {
            Id = booking.Id,
            HotelId = booking.HotelId,
            HotelName = booking.Hotel.Name,
            GuestName = $"{booking.User.FirstName} {booking.User.LastName}",
            GuestEmail = booking.User.Email ?? string.Empty,
            GuestPhone = booking.User.PhoneNumber ?? string.Empty,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            RoomType = booking.RoomType,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            PaymentStatus = booking.PaymentStatus,
            SpecialRequests = booking.SpecialRequests,
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<List<HotelBookingCountDto>> GetBookingCountsPerHotelAsync(string ownerId)
    {
        var hotels = await _context.Hotels
            .Where(h => h.OwnerId == ownerId)
            .ToListAsync();

        var hotelIds = hotels.Select(h => h.Id).ToList();

        var bookings = await _context.HotelBookings
            .Where(b => hotelIds.Contains(b.HotelId))
            .ToListAsync();

        return hotels.Select(h => new HotelBookingCountDto
        {
            HotelId = h.Id,
            HotelName = h.Name,
            TotalBookings = bookings.Count(b => b.HotelId == h.Id),
            PendingBookings = bookings.Count(b => b.HotelId == h.Id && (b.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) || b.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))),
            ConfirmedBookings = bookings.Count(b => b.HotelId == h.Id && b.Status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase)),
            RejectedBookings = bookings.Count(b => b.HotelId == h.Id && (b.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase) || b.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)))
        }).ToList();
    }
}
