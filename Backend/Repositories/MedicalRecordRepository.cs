using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly NileCareDbContextV2 _context;

    public MedicalRecordRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<MedicalRecord>> GetByUserIdAsync(string userId)
    {
        return await _context.MedicalRecords
            .Where(mr => mr.UserId == userId)
            .OrderByDescending(mr => mr.UploadedAt)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetByIdAsync(int id)
    {
        return await _context.MedicalRecords.FindAsync(id);
    }

    public async Task<MedicalRecord> CreateAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record != null)
        {
            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}


