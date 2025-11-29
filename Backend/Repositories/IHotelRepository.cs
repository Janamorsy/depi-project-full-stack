using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(int id);
    Task<List<Hotel>> SearchAsync(string? city, bool? wheelchairAccessible);
}
