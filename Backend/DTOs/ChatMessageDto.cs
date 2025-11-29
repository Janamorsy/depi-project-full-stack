namespace NileCareAPI.DTOs;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string? PatientProfilePicture { get; set; }
    public string DoctorId { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string? DoctorProfilePicture { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? FileUrl { get; set; } // Add this

    public string SenderType { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ChatUserDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = "/images/default-avatar.png";
}

public class ChatListItemDto
{
    public int Id { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public ChatUserDto? Patient { get; set; }
    public ChatUserDto? Doctor { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public string SenderType { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SendMessageDto
{
   
        public string Message { get; set; }
        public string RecipientId { get; set; }
        public IFormFile? File { get; set; }
    }




