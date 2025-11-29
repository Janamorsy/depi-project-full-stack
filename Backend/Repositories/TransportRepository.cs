using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class TransportRepository : ITransportRepository
{
    private readonly NileCareDbContextV2 _context;

    public TransportRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<Transport>> GetAllAsync()
    {
        return await _context.Transports.ToListAsync();
    }

    public async Task<Transport?> GetByIdAsync(int id)
    {
        return await _context.Transports.FindAsync(id);
    }

    public async Task<List<Transport>> GetAccessibleAsync()
    {
        return await _context.Transports
            .Where(t => t.WheelchairAccessible)
            .ToListAsync();
    }
}


