using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelAuthController : ControllerBase
{
    private readonly IHotelAuthService _hotelAuthService;

    public HotelAuthController(IHotelAuthService hotelAuthService)
    {
        _hotelAuthService = hotelAuthService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(HotelUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] HotelUserRegisterDto registerDto)
    {
        var hotelUser = await _hotelAuthService.RegisterAsync(registerDto);
        if (hotelUser == null)
            return BadRequest(new { message = "Registration failed. Email may already be in use." });

        return Ok(hotelUser);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(HotelUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var hotelUser = await _hotelAuthService.LoginAsync(loginDto);
        if (hotelUser == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(hotelUser);
    }
}
