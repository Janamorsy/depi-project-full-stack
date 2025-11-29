using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }


    [HttpGet("search")]
    [ProducesResponseType(typeof(List<DoctorSearchResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SearchDoctors(
        [FromQuery] string? city,
        [FromQuery] string? specialty,
        [FromQuery] string? language)
    {
        var doctors = await _doctorService.SearchDoctorsAsync(city, specialty, language);
        return Ok(doctors);
    }
}


