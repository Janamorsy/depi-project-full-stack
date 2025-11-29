using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly NileCareDbContextV2 _context;

    public ChatMessageRepository(NileCareDbContextV2 context)
    {
        _context = context;
    }

    public async Task<List<ChatMessage>> GetConversationAsync(string patientId, string doctorId)
    {
        return await _context.ChatMessages
            .AsNoTracking()
            .Where(cm => cm.PatientId == patientId && cm.DoctorId == doctorId)
            .OrderByDescending(cm => cm.Id)
            .Take(50)
            .OrderBy(cm => cm.Id)
            .ToListAsync();
    }

    public async Task<List<ChatMessage>> GetAllChatsAsync(string userId)
    {
        var chats = await _context.ChatMessages
            .AsNoTracking()
            .Where(cm => cm.PatientId == userId || cm.DoctorId == userId)
            .Include(cm => cm.Patient)
            .Include(cm => cm.Doctor)
            .OrderByDescending(cm => cm.CreatedAt)
            .ToListAsync();

        // Determine if user is patient or doctor for each conversation
        var latestChats = chats
            .GroupBy(cm => new { cm.PatientId, cm.DoctorId })
            .Select(g => {
                var latest = g.First();
                // Check if there are unread messages FROM the other person
                bool isUserPatient = latest.PatientId == userId;
                // If user is patient, check unread messages from doctor (SenderType = "Doctor")
                // If user is doctor, check unread messages from patient (SenderType = "Patient")
                string otherSenderType = isUserPatient ? "Doctor" : "Patient";
                bool hasUnread = g.Any(m => m.SenderType == otherSenderType && !m.IsRead);
                latest.IsRead = !hasUnread; // IsRead = true means NO unread messages
                return latest;
            })
            .OrderByDescending(cm => cm.CreatedAt)
            .ToList();

        foreach (var chat in latestChats)
        {
            if (chat.Patient != null)
            {
                chat.Patient.ChatMessages = null;
            }
            if (chat.Doctor != null)
            {
                chat.Doctor.ChatMessages = null;
            }
        }

        return latestChats;
    }



    public async Task<List<ChatMessage>> GetUserConversationsAsync(string userId, bool isDoctor)
    {
        return await _context.ChatMessages
            .AsNoTracking()
            .Where(cm => isDoctor ? cm.DoctorId == userId : cm.PatientId == userId)
            .OrderByDescending(cm => cm.Id)
            .Take(50)
            .ToListAsync();
    }

    public async Task<ChatMessage> CreateAsync(ChatMessage message)
    {
        message.CreatedAt = DateTime.UtcNow;
        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task MarkAsReadAsync(int messageId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE ChatMessages SET IsRead = 1 WHERE Id = {0}", messageId);
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.ChatMessages
            .AsNoTracking()
            .Where(cm => (cm.PatientId == userId || cm.DoctorId == userId) && !cm.IsRead)
            .CountAsync();
    }
}


