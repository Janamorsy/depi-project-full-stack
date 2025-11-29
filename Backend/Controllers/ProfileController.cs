using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.Models;
using System.Security.Claims;

namespace NileCareAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPut("picture")]
    public async Task<IActionResult> UpdateProfilePicture([FromBody] UpdateProfilePictureRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        user.ProfilePicture = string.IsNullOrWhiteSpace(request.ProfilePicture)
            ? "/images/default-avatar.png"
            : request.ProfilePicture;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to update profile picture" });

        return Ok(new { profilePicture = user.ProfilePicture });
    }
}

public class UpdateProfilePictureRequest
{
    public string ProfilePicture { get; set; } = string.Empty;
}

