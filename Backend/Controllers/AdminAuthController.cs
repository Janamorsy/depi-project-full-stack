using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
public class AdminAuthController : ControllerBase
{
    private readonly IAdminAuthService _adminAuthService;

    public AdminAuthController(IAdminAuthService adminAuthService)
    {
        _adminAuthService = adminAuthService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AdminDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] AdminRegisterDto registerDto)
    {
        var admin = await _adminAuthService.RegisterAsync(registerDto);
        if (admin == null)
            return BadRequest(new { message = "Registration failed. Invalid admin secret or email already exists." });

        return Ok(admin);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AdminDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto loginDto)
    {
        var admin = await _adminAuthService.LoginAsync(loginDto);
        if (admin == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(admin);
    }
}
