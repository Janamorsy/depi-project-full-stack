using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[ApiController]
[Route("api/[controller]")]
public class DoctorAuthController : ControllerBase
{
    private readonly IDoctorAuthService _doctorAuthService;

    public DoctorAuthController(IDoctorAuthService doctorAuthService)
    {
        _doctorAuthService = doctorAuthService;
    }



    [HttpPost("register")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] DoctorRegisterDto registerDto)
    {
        try
        {
            var doctor = await _doctorAuthService.RegisterAsync(registerDto);
            if (doctor == null)
                return BadRequest(new { message = "Registration failed" });

            return Ok(doctor);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



    [HttpPost("login")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var doctor = await _doctorAuthService.LoginAsync(loginDto);
        if (doctor == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(doctor);
    }
}


