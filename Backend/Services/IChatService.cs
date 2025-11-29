using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public interface IChatService
{
    Task SendMessageAsync(SendMessageDto messageDto, bool isDoctor);
    Task<List<ChatMessageDto>> GetConversationAsync(string otherUserId, int? lastMessageId);
    Task<List<ChatMessageDto>> GetAllConversationsAsync();
    Task MarkAsReadAsync(int messageId);
    Task MarkConversationAsReadAsync(string otherUserId, bool isDoctor);
    Task<List<ChatListItemDto>> GetAllChatsAsync(string userId);
}


