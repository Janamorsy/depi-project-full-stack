using Microsoft.AspNetCore.Http;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public interface IAdminService
{
    // Dashboard Stats
    Task<AdminDashboardStatsDto> GetDashboardStatsAsync();
    
    // User Management
    Task<List<UserListDto>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(string userId);
    
    // Hotel Management
    Task<List<Hotel>> GetAllHotelsAsync();
    Task<Hotel?> GetHotelByIdAsync(int id);
    Task<Hotel> CreateHotelAsync(CreateHotelDto dto, List<IFormFile>? images);
    Task<Hotel?> UpdateHotelAsync(int id, UpdateHotelDto dto, List<IFormFile>? newImages, List<int>? deleteImageIds);
    Task<bool> DeleteHotelAsync(int id);
    
    // Hotel Approval Management
    Task<List<PendingHotelDto>> GetPendingHotelsAsync();
    Task<Hotel?> ApproveHotelAsync(int id);
    Task<Hotel?> RejectHotelAsync(int id, string reason);
    
    // Transport Management
    Task<List<Transport>> GetAllTransportsAsync();
    Task<Transport?> GetTransportByIdAsync(int id);
    Task<Transport> CreateTransportAsync(CreateTransportDto dto, IFormFile? image);
    Task<Transport?> UpdateTransportAsync(int id, UpdateTransportDto dto, IFormFile? newImage);
    Task<bool> DeleteTransportAsync(int id);
}
