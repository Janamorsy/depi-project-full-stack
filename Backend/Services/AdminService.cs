using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Services;

public class AdminService : IAdminService
{
    private readonly NileCareDbContextV2 _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHotelRepository _hotelRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly IWebHostEnvironment _environment;

    public AdminService(
        NileCareDbContextV2 context,
        UserManager<ApplicationUser> userManager,
        IHotelRepository hotelRepository,
        ITransportRepository transportRepository,
        IWebHostEnvironment environment)
    {
        _context = context;
        _userManager = userManager;
        _hotelRepository = hotelRepository;
        _transportRepository = transportRepository;
        _environment = environment;
    }

    private async Task<string> SaveImageAsync(IFormFile file, string folder)
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    private void DeleteImageFile(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/uploads/"))
            return;

        var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    public async Task<AdminDashboardStatsDto> GetDashboardStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalDoctors = await _context.DoctorUsers.CountAsync();
        var totalHotels = await _context.Hotels.CountAsync(h => h.ApprovalStatus == "Approved");
        var totalTransports = await _context.Transports.CountAsync();
        var totalAppointments = await _context.Appointments.CountAsync();
        var totalHotelBookings = await _context.HotelBookings.CountAsync();
        var totalTransportBookings = await _context.TransportBookings.CountAsync();
        var pendingHotelRequests = await _context.Hotels.CountAsync(h => h.ApprovalStatus == "Pending");

        var paidPayments = await _context.Payments
            .Where(p => p.Status == "Paid")
            .SumAsync(p => p.Amount);

        return new AdminDashboardStatsDto
        {
            TotalUsers = totalUsers,
            TotalDoctors = totalDoctors,
            TotalHotels = totalHotels,
            TotalTransports = totalTransports,
            TotalAppointments = totalAppointments,
            TotalHotelBookings = totalHotelBookings,
            TotalTransportBookings = totalTransportBookings,
            TotalRevenue = paidPayments,
            PendingHotelRequests = pendingHotelRequests
        };
    }

    // User Management
    public async Task<List<UserListDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Appointments)
            .Include(u => u.HotelBookings)
            .Include(u => u.TransportBookings)
            .Select(u => new UserListDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                ProfilePicture = u.ProfilePicture,
                IsWheelchairAccessible = u.IsWheelchairAccessible,
                AppointmentsCount = u.Appointments.Count,
                HotelBookingsCount = u.HotelBookings.Count,
                TransportBookingsCount = u.TransportBookings.Count
            })
            .ToListAsync();

        return users;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    // Hotel Management
    public async Task<List<Hotel>> GetAllHotelsAsync()
    {
        return await _context.Hotels.Include(h => h.Images.OrderBy(i => i.DisplayOrder)).ToListAsync();
    }

    public async Task<Hotel?> GetHotelByIdAsync(int id)
    {
        return await _context.Hotels.Include(h => h.Images.OrderBy(i => i.DisplayOrder)).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Hotel> CreateHotelAsync(CreateHotelDto dto, List<IFormFile>? images)
    {
        var hotel = new Hotel
        {
            Name = dto.Name,
            City = dto.City,
            Address = dto.Address,
            PricePerNight = dto.PricePerNight,
            Rating = dto.Rating,
            WheelchairAccessible = dto.WheelchairAccessible,
            RollInShower = dto.RollInShower,
            ElevatorAccess = dto.ElevatorAccess,
            GrabBars = dto.GrabBars,
            Amenities = dto.Amenities,
            Description = dto.Description,
            StandardRoomPrice = dto.StandardRoomPrice,
            StandardRoomMaxGuests = dto.StandardRoomMaxGuests,
            DeluxeRoomPrice = dto.DeluxeRoomPrice,
            DeluxeRoomMaxGuests = dto.DeluxeRoomMaxGuests,
            SuiteRoomPrice = dto.SuiteRoomPrice,
            SuiteRoomMaxGuests = dto.SuiteRoomMaxGuests,
            FamilyRoomPrice = dto.FamilyRoomPrice,
            FamilyRoomMaxGuests = dto.FamilyRoomMaxGuests
        };

        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        // Handle image uploads
        if (images != null && images.Count > 0)
        {
            var order = 0;
            foreach (var image in images)
            {
                var imageUrl = await SaveImageAsync(image, "hotels");
                var hotelImage = new HotelImage
                {
                    HotelId = hotel.Id,
                    ImageUrl = imageUrl,
                    DisplayOrder = order++
                };
                _context.HotelImages.Add(hotelImage);
                
                // Set first image as primary
                if (order == 1)
                    hotel.ImageUrl = imageUrl;
            }
            await _context.SaveChangesAsync();
        }

        return await GetHotelByIdAsync(hotel.Id) ?? hotel;
    }

    public async Task<Hotel?> UpdateHotelAsync(int id, UpdateHotelDto dto, List<IFormFile>? newImages, List<int>? deleteImageIds)
    {
        var hotel = await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
            return null;

        hotel.Name = dto.Name;
        hotel.City = dto.City;
        hotel.Address = dto.Address;
        hotel.PricePerNight = dto.PricePerNight;
        hotel.Rating = dto.Rating;
        hotel.WheelchairAccessible = dto.WheelchairAccessible;
        hotel.RollInShower = dto.RollInShower;
        hotel.ElevatorAccess = dto.ElevatorAccess;
        hotel.GrabBars = dto.GrabBars;
        hotel.Amenities = dto.Amenities;
        hotel.Description = dto.Description;
        hotel.StandardRoomPrice = dto.StandardRoomPrice;
        hotel.StandardRoomMaxGuests = dto.StandardRoomMaxGuests;
        hotel.DeluxeRoomPrice = dto.DeluxeRoomPrice;
        hotel.DeluxeRoomMaxGuests = dto.DeluxeRoomMaxGuests;
        hotel.SuiteRoomPrice = dto.SuiteRoomPrice;
        hotel.SuiteRoomMaxGuests = dto.SuiteRoomMaxGuests;
        hotel.FamilyRoomPrice = dto.FamilyRoomPrice;
        hotel.FamilyRoomMaxGuests = dto.FamilyRoomMaxGuests;

        // Delete specified images
        if (deleteImageIds != null && deleteImageIds.Count > 0)
        {
            var imagesToDelete = hotel.Images.Where(i => deleteImageIds.Contains(i.Id)).ToList();
            foreach (var img in imagesToDelete)
            {
                DeleteImageFile(img.ImageUrl);
                _context.HotelImages.Remove(img);
            }
        }

        // Add new images
        if (newImages != null && newImages.Count > 0)
        {
            var maxOrder = hotel.Images.Any() ? hotel.Images.Max(i => i.DisplayOrder) + 1 : 0;
            foreach (var image in newImages)
            {
                var imageUrl = await SaveImageAsync(image, "hotels");
                var hotelImage = new HotelImage
                {
                    HotelId = hotel.Id,
                    ImageUrl = imageUrl,
                    DisplayOrder = maxOrder++
                };
                _context.HotelImages.Add(hotelImage);
            }
        }

        await _context.SaveChangesAsync();

        // Update primary image if needed
        var updatedHotel = await GetHotelByIdAsync(id);
        if (updatedHotel != null && updatedHotel.Images.Any())
        {
            updatedHotel.ImageUrl = updatedHotel.Images.OrderBy(i => i.DisplayOrder).First().ImageUrl;
            await _context.SaveChangesAsync();
        }

        return updatedHotel;
    }

    public async Task<bool> DeleteHotelAsync(int id)
    {
        var hotel = await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
            return false;

        // Delete all image files
        foreach (var img in hotel.Images)
        {
            DeleteImageFile(img.ImageUrl);
        }

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
        return true;
    }

    // Hotel Approval Management
    public async Task<List<PendingHotelDto>> GetPendingHotelsAsync()
    {
        return await _context.Hotels
            .Include(h => h.Images)
            .Include(h => h.Owner)
            .Where(h => h.ApprovalStatus == "Pending")
            .OrderBy(h => h.CreatedAt)
            .Select(h => new PendingHotelDto
            {
                Id = h.Id,
                Name = h.Name,
                City = h.City,
                Address = h.Address,
                Description = h.Description,
                Amenities = h.Amenities,
                StandardRoomPrice = h.StandardRoomPrice,
                WheelchairAccessible = h.WheelchairAccessible,
                RollInShower = h.RollInShower,
                ElevatorAccess = h.ElevatorAccess,
                GrabBars = h.GrabBars,
                ApprovalStatus = h.ApprovalStatus,
                RejectionReason = h.RejectionReason,
                CreatedAt = h.CreatedAt,
                OwnerId = h.OwnerId ?? string.Empty,
                OwnerName = h.Owner != null ? $"{h.Owner.FirstName} {h.Owner.LastName}" : "Unknown",
                OwnerEmail = h.Owner != null ? h.Owner.Email ?? string.Empty : string.Empty,
                OwnerCompany = h.Owner != null ? h.Owner.CompanyName : string.Empty,
                Images = h.Images.Select(i => new HotelImageDto { Id = i.Id, ImageUrl = i.ImageUrl }).ToList()
            })
            .ToListAsync();
    }

    public async Task<Hotel?> ApproveHotelAsync(int id)
    {
        var hotel = await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
            return null;

        hotel.ApprovalStatus = "Approved";
        hotel.RejectionReason = null;
        await _context.SaveChangesAsync();
        return hotel;
    }

    public async Task<Hotel?> RejectHotelAsync(int id, string reason)
    {
        var hotel = await _context.Hotels.Include(h => h.Images).FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
            return null;

        hotel.ApprovalStatus = "Rejected";
        hotel.RejectionReason = reason;
        await _context.SaveChangesAsync();
        return hotel;
    }

    // Transport Management
    public async Task<List<Transport>> GetAllTransportsAsync()
    {
        return await _transportRepository.GetAllAsync();
    }

    public async Task<Transport?> GetTransportByIdAsync(int id)
    {
        return await _transportRepository.GetByIdAsync(id);
    }

    public async Task<Transport> CreateTransportAsync(CreateTransportDto dto, IFormFile? image)
    {
        var transport = new Transport
        {
            VehicleType = dto.VehicleType,
            WheelchairAccessible = dto.WheelchairAccessible,
            Capacity = dto.Capacity,
            PricePerHour = dto.PricePerHour,
            Description = dto.Description,
            Features = dto.Features
        };

        if (image != null)
        {
            transport.ImageUrl = await SaveImageAsync(image, "transports");
        }

        await _context.Transports.AddAsync(transport);
        await _context.SaveChangesAsync();
        return transport;
    }

    public async Task<Transport?> UpdateTransportAsync(int id, UpdateTransportDto dto, IFormFile? newImage)
    {
        var transport = await _transportRepository.GetByIdAsync(id);
        if (transport == null)
            return null;

        transport.VehicleType = dto.VehicleType;
        transport.WheelchairAccessible = dto.WheelchairAccessible;
        transport.Capacity = dto.Capacity;
        transport.PricePerHour = dto.PricePerHour;
        transport.Description = dto.Description;
        transport.Features = dto.Features;

        if (newImage != null)
        {
            // Delete old image
            DeleteImageFile(transport.ImageUrl);
            transport.ImageUrl = await SaveImageAsync(newImage, "transports");
        }

        _context.Transports.Update(transport);
        await _context.SaveChangesAsync();
        return transport;
    }

    public async Task<bool> DeleteTransportAsync(int id)
    {
        var transport = await _transportRepository.GetByIdAsync(id);
        if (transport == null)
            return false;

        DeleteImageFile(transport.ImageUrl);
        _context.Transports.Remove(transport);
        await _context.SaveChangesAsync();
        return true;
    }
}
