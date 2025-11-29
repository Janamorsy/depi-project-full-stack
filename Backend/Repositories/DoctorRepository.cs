using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly NileCareDbContextV2 _context;

    public DoctorRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors
            .Include(d => d.AvailabilitySlots)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<Doctor>> SearchAsync(string? city, string? specialty, string? language)
    {
        var query = _context.Doctors
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
