using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly NileCareDbContextV2 _context;

    public HotelRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<Hotel?> GetByIdAsync(int id)
    {
        return await _context.Hotels
            .Include(h => h.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(h => h.Id == id && h.ApprovalStatus == "Approved");
    }

    public async Task<List<Hotel>> SearchAsync(string? city, bool? wheelchairAccessible)
    {
        var query = _context.Hotels
            .Include(h => h.Images.OrderBy(i => i.DisplayOrder))
            .Where(h => h.ApprovalStatus == "Approved")
            .AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(h => h.City.Contains(city));

        if (wheelchairAccessible.HasValue && wheelchairAccessible.Value)
            query = query.Where(h => h.WheelchairAccessible);

        return await query.ToListAsync();
    }
}