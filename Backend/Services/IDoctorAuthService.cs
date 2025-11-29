using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IDoctorAuthService
{
    Task<DoctorDto?> RegisterAsync(DoctorRegisterDto registerDto);
    Task<DoctorDto?> LoginAsync(LoginDto loginDto);
}


