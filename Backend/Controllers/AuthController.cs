using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NileCareAPI.DTOs;
using NileCareAPI.Services;
using System.Security.Claims;

namespace NileCareAPI.Controllers;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }





    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        if (user == null)
            return BadRequest(new { message = "Registration failed" });

        return Ok(user);
    }





    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(loginDto);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(user);
    }





    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "User not authenticated" });

        var user = await _authService.UpdateProfileAsync(userId, updateDto);
        if (user == null)
            return BadRequest(new { message = "Profile update failed" });

        return Ok(user);
    }
}


