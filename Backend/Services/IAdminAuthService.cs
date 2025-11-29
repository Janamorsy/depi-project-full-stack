using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IAdminAuthService
{
    Task<AdminDto?> RegisterAsync(AdminRegisterDto registerDto);
    Task<AdminDto?> LoginAsync(AdminLoginDto loginDto);
}
