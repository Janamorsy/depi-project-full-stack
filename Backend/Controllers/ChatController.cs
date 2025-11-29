
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromForm] SendMessageDto messageDto)
    {
        if (messageDto == null)
            return BadRequest("MessageDto is null");

        Console.WriteLine($"RecipientId: {messageDto.RecipientId}");
        Console.WriteLine($"Message: {messageDto.Message}");
        Console.WriteLine($"File: {(messageDto.File != null ? messageDto.File.FileName : "null")}");

        var isDoctor = User.IsInRole("Doctor");
        await _chatService.SendMessageAsync(messageDto, isDoctor);
        return Ok(new { success = true });
    }



    [HttpGet("conversation/{otherUserId}")]
    public async Task<IActionResult> GetConversation(string otherUserId, [FromQuery] int? lastMessageId)
    {
        var messages = await _chatService.GetConversationAsync(otherUserId, lastMessageId);
        return Ok(messages);
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var messages = await _chatService.GetAllConversationsAsync();
        return Ok(messages);
    }

    [HttpPut("{messageId}/read")]
    public async Task<IActionResult> MarkAsRead(int messageId)
    {
        await _chatService.MarkAsReadAsync(messageId);
        return Ok();
    }

    [HttpPut("conversation/{otherUserId}/read")]
    public async Task<IActionResult> MarkConversationAsRead(string otherUserId)
    {
        var isDoctor = User.IsInRole("Doctor");
        await _chatService.MarkConversationAsReadAsync(otherUserId, isDoctor);
        return Ok();
    }


    [HttpGet("{userId}/all")]
    public async Task<IActionResult> GetAllChats(string userId)
    {
        // Security: Validate user can only access their own chats
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId)
        {
            return Forbid();
        }

        try
        {
            var chats = await _chatService.GetAllChatsAsync(userId);
            return Ok(chats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



}



