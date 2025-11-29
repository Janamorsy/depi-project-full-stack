using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IAuthService
{
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> UpdateProfileAsync(string userId, UpdateProfileDto updateDto);
}


