using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class HotelBookingRepository : IHotelBookingRepository
{
    private readonly NileCareDbContextV2 _context;

    public HotelBookingRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<HotelBooking>> GetByUserIdAsync(string userId)
    {
        return await _context.HotelBookings
            .Include(hb => hb.Hotel)
            .Where(hb => hb.UserId == userId)
            .OrderByDescending(hb => hb.CheckInDate)
            .ToListAsync();
    }

    public async Task<HotelBooking?> GetByIdAsync(int id)
    {
        return await _context.HotelBookings
            .Include(hb => hb.Hotel)
            .FirstOrDefaultAsync(hb => hb.Id == id);
    }

    public async Task<HotelBooking> CreateAsync(HotelBooking booking)
    {
        _context.HotelBookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<HotelBooking> UpdateAsync(HotelBooking booking)
    {
        _context.HotelBookings.Update(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task DeleteAsync(int id)
    {
        var booking = await _context.HotelBookings.FindAsync(id);
        if (booking != null)
        {
            _context.HotelBookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}


