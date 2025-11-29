using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IHotelAuthService
{
    Task<HotelUserDto?> RegisterAsync(HotelUserRegisterDto registerDto);
    Task<HotelUserDto?> LoginAsync(LoginDto loginDto);
}
