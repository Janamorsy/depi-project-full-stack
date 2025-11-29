using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalProfileController : ControllerBase
{
    private readonly IMedicalProfileService _medicalProfileService;

    public MedicalProfileController(IMedicalProfileService medicalProfileService)
    {
        _medicalProfileService = medicalProfileService;
    }




    [HttpGet]
    [ProducesResponseType(typeof(MedicalProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _medicalProfileService.GetProfileAsync();
        if (profile == null)
            return NotFound();

        return Ok(profile);
    }





    [HttpPost]
    [ProducesResponseType(typeof(MedicalProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOrUpdateProfile([FromBody] MedicalProfileDto profileDto)
    {
        var profile = await _medicalProfileService.CreateOrUpdateProfileAsync(profileDto);
        return Ok(profile);
    }
}


