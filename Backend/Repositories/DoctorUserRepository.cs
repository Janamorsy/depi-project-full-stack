using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class DoctorUserRepository : IDoctorUserRepository
{
    private readonly NileCareDbContextV2 _context;

    public DoctorUserRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<DoctorUser?> GetByIdAsync(string id)
    {
        return await _context.DoctorUsers
            .Include(d => d.AvailabilitySlots)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<DoctorUser>> SearchAsync(string? city, string? specialty, string? language)
    {
        var query = _context.DoctorUsers
            .Where(d => d.IsAvailable)
            .Include(d => d.AvailabilitySlots)
            .AsQueryable();

        if (!string.IsNullOrEmpty(city))
            query = query.Where(d => d.City.Contains(city));

        if (!string.IsNullOrEmpty(specialty))
            query = query.Where(d => d.Specialty.Contains(specialty));

        if (!string.IsNullOrEmpty(language))
            query = query.Where(d => d.Languages.Contains(language));

        return await query.ToListAsync();
    }
}
