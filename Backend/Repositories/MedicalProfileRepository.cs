using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class MedicalProfileRepository : IMedicalProfileRepository
{
    private readonly NileCareDbContextV2 _context;

    public MedicalProfileRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<MedicalProfile?> GetByUserIdAsync(string userId)
    {
        return await _context.MedicalProfiles.FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<MedicalProfile> CreateAsync(MedicalProfile profile)
    {
        _context.MedicalProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<MedicalProfile> UpdateAsync(MedicalProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        _context.MedicalProfiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }
}


