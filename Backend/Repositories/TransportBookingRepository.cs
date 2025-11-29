using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class TransportBookingRepository : ITransportBookingRepository
{
    private readonly NileCareDbContextV2 _context;

    public TransportBookingRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<TransportBooking>> GetByUserIdAsync(string userId)
    {
        return await _context.TransportBookings
            .Include(tb => tb.Transport)
            .Where(tb => tb.UserId == userId)
            .OrderByDescending(tb => tb.PickupDateTime)
            .ToListAsync();
    }

    public async Task<TransportBooking?> GetByIdAsync(int id)
    {
        return await _context.TransportBookings
            .Include(tb => tb.Transport)
            .FirstOrDefaultAsync(tb => tb.Id == id);
    }

    public async Task<TransportBooking> CreateAsync(TransportBooking booking)
    {
        _context.TransportBookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<TransportBooking> UpdateAsync(TransportBooking booking)
    {
        _context.TransportBookings.Update(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task DeleteAsync(int id)
    {
        var booking = await _context.TransportBookings.FindAsync(id);
        if (booking != null)
        {
            _context.TransportBookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}


