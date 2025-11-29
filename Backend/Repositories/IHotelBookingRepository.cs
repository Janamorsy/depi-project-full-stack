using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IHotelBookingRepository
{
    Task<List<HotelBooking>> GetByUserIdAsync(string userId);
    Task<HotelBooking?> GetByIdAsync(int id);
    Task<HotelBooking> CreateAsync(HotelBooking booking);
    Task<HotelBooking> UpdateAsync(HotelBooking booking);
    Task DeleteAsync(int id);
}


