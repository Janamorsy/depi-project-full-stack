
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;
using NileCareAPI.Data;

namespace NileCareAPI.Services;

public class ChatService : IChatService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly NileCareDbContextV2 _context;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _env;

    public ChatService(
        IHttpContextAccessor httpContextAccessor,
        IChatMessageRepository chatMessageRepository,
        NileCareDbContextV2 context,
        IMemoryCache cache,
        IWebHostEnvironment env)
    {
        _httpContextAccessor = httpContextAccessor;
        _chatMessageRepository = chatMessageRepository;
        _context = context;
        _cache = cache;
        _env = env;
    }

    private string GetUserId()
        => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();

    private bool IsDoctor()
        => _httpContextAccessor.HttpContext?.User.IsInRole("Doctor") ?? false;
    public async Task SendMessageAsync(SendMessageDto messageDto, bool isDoctor)
    {
        if (messageDto == null)
            throw new ArgumentNullException(nameof(messageDto));

        var userId = GetUserId();

        var chatMessage = new ChatMessage
        {
            Message = messageDto.Message?.Trim(), 
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            PatientId = isDoctor ? messageDto.RecipientId : userId,
            DoctorId = isDoctor ? userId : messageDto.RecipientId,
            SenderType = isDoctor ? "Doctor" : "Patient",
            FileUrl = null 
        };

        if (messageDto.File != null && messageDto.File.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(messageDto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await messageDto.File.CopyToAsync(stream);
            }
            chatMessage.FileUrl = $"/uploads/{fileName}";
        }
        if (string.IsNullOrWhiteSpace(chatMessage.Message) && chatMessage.FileUrl == null)
        {
            throw new InvalidOperationException("Cannot send an empty message without a file.");
        }

        await _chatMessageRepository.CreateAsync(chatMessage);
    }


    public async Task<List<ChatMessageDto>> GetConversationAsync(string otherUserId, int? lastMessageId)
    {
        var userId = GetUserId();
        var isDoctor = IsDoctor();

        var query = _context.ChatMessages.AsNoTracking();

        if (isDoctor)
            query = query.Where(m => m.PatientId == otherUserId && m.DoctorId == userId);
        else
            query = query.Where(m => m.PatientId == userId && m.DoctorId == otherUserId);

        if (lastMessageId.HasValue)
            query = query.Where(m => m.Id > lastMessageId.Value);

        var messages = await query
            .OrderBy(m => m.Id)
            .Take(50)
            .ToListAsync();

        if (!messages.Any())
            return new List<ChatMessageDto>();

        var patientId = messages.First().PatientId;
        var doctorId = messages.First().DoctorId;

        var patient = await _cache.GetOrCreateAsync($"user_{patientId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _context.Users.AsNoTracking()
                .Where(u => u.Id == patientId)
                .Select(u => new { u.FirstName, u.LastName, u.ProfilePicture })
                .FirstOrDefaultAsync();
        });

        var doctor = await _cache.GetOrCreateAsync($"doctor_{doctorId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _context.DoctorUsers.AsNoTracking()
                .Where(d => d.Id == doctorId)
                .Select(d => new { d.FirstName, d.LastName, d.ImageUrl })
                .FirstOrDefaultAsync();
        });

        return messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            PatientName = patient != null ? $"{patient.FirstName} {patient.LastName}" : "Patient",
            PatientProfilePicture = patient?.ProfilePicture,
            DoctorId = m.DoctorId,
            DoctorName = doctor != null ? $"Dr. {doctor.FirstName} {doctor.LastName}" : "Doctor",
            DoctorProfilePicture = doctor?.ImageUrl,
            Message = m.Message,
            FileUrl = m.FileUrl,
            SenderType = m.SenderType,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt
        }).ToList();
    }

    public async Task<List<ChatMessageDto>> GetAllConversationsAsync()
    {
        var messages = await _chatMessageRepository.GetUserConversationsAsync(GetUserId(), IsDoctor());

        return messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            DoctorId = m.DoctorId,
            Message = m.Message,
            FileUrl = m.FileUrl,
            SenderType = m.SenderType,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt
        }).ToList();
    }

    public async Task MarkAsReadAsync(int messageId)
    {
        await _chatMessageRepository.MarkAsReadAsync(messageId);
    }

    public async Task MarkConversationAsReadAsync(string otherUserId, bool isDoctor)
    {
        var userId = GetUserId();
        
        // Mark all messages FROM the other user as read (messages I received)
        if (isDoctor)
        {
            // Doctor is reading - mark patient's messages as read
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE ChatMessages SET IsRead = 1 WHERE PatientId = {0} AND DoctorId = {1} AND SenderType = 'Patient' AND IsRead = 0",
                otherUserId, userId);
        }
        else
        {
            // Patient is reading - mark doctor's messages as read
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE ChatMessages SET IsRead = 1 WHERE PatientId = {0} AND DoctorId = {1} AND SenderType = 'Doctor' AND IsRead = 0",
                userId, otherUserId);
        }
    }

    public async Task<List<ChatListItemDto>> GetAllChatsAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("User ID cannot be null");

        var chats = await _chatMessageRepository.GetAllChatsAsync(userId);
        
        return chats.Select(m => new ChatListItemDto
        {
            Id = m.Id,
            PatientId = m.PatientId,
            DoctorId = m.DoctorId,
            Patient = m.Patient != null ? new ChatUserDto
            {
                Id = m.Patient.Id,
                FirstName = m.Patient.FirstName,
                LastName = m.Patient.LastName,
                ProfilePicture = m.Patient.ProfilePicture ?? "/images/default-avatar.png"
            } : null,
            Doctor = m.Doctor != null ? new ChatUserDto
            {
                Id = m.Doctor.Id,
                FirstName = m.Doctor.FirstName,
                LastName = m.Doctor.LastName,
                ProfilePicture = m.Doctor.ImageUrl ?? "/images/default-avatar.png"
            } : null,
            Message = m.Message,
            FileUrl = m.FileUrl,
            SenderType = m.SenderType,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt
        }).ToList();
    }

}
