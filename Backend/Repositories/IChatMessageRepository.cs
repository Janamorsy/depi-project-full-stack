using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Repositories;

public interface IChatMessageRepository
{
    Task<List<ChatMessage>> GetConversationAsync(string patientId, string doctorId);
    Task<List<ChatMessage>> GetUserConversationsAsync(string userId, bool isDoctor);
    Task<ChatMessage> CreateAsync(ChatMessage message);
    Task<List<ChatMessage>> GetAllChatsAsync(string userId);

    Task MarkAsReadAsync(int messageId);
    Task<int> GetUnreadCountAsync(string userId);
}


