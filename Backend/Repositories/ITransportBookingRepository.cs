using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface ITransportBookingRepository
{
    Task<List<TransportBooking>> GetByUserIdAsync(string userId);
    Task<TransportBooking?> GetByIdAsync(int id);
    Task<TransportBooking> CreateAsync(TransportBooking booking);
    Task<TransportBooking> UpdateAsync(TransportBooking booking);
    Task DeleteAsync(int id);
}


