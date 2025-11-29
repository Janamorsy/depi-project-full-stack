using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // Dashboard Stats
    [HttpGet("dashboard/stats")]
    [ProducesResponseType(typeof(AdminDashboardStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = await _adminService.GetDashboardStatsAsync();
        return Ok(stats);
    }

    // User Management
    [HttpGet("users")]
    [ProducesResponseType(typeof(List<UserListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _adminService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpDelete("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _adminService.DeleteUserAsync(userId);
        if (!result)
            return NotFound(new { message = "User not found" });

        return Ok(new { message = "User deleted successfully" });
    }

    // Hotel Management
    [HttpGet("hotels")]
    [ProducesResponseType(typeof(List<NileCareAPI.Models.Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHotels()
    {
        var hotels = await _adminService.GetAllHotelsAsync();
        return Ok(hotels);
    }

    [HttpGet("hotels/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _adminService.GetHotelByIdAsync(id);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(hotel);
    }

    [HttpPost("hotels")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateHotel([FromForm] CreateHotelDto dto, [FromForm] List<IFormFile>? images)
    {
        var hotel = await _adminService.CreateHotelAsync(dto, images);
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("hotels/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel(int id, [FromForm] UpdateHotelDto dto, [FromForm] List<IFormFile>? images, [FromForm] List<int>? deleteImageIds)
    {
        var hotel = await _adminService.UpdateHotelAsync(id, dto, images, deleteImageIds);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(hotel);
    }

    [HttpDelete("hotels/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await _adminService.DeleteHotelAsync(id);
        if (!result)
            return NotFound(new { message = "Hotel not found" });

        return Ok(new { message = "Hotel deleted successfully" });
    }

    // Hotel Approval Management
    [HttpGet("hotels/pending")]
    [ProducesResponseType(typeof(List<NileCareAPI.DTOs.PendingHotelDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingHotels()
    {
        var hotels = await _adminService.GetPendingHotelsAsync();
        return Ok(hotels);
    }

    [HttpPost("hotels/{id}/approve")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveHotel(int id)
    {
        var hotel = await _adminService.ApproveHotelAsync(id);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(new { message = "Hotel approved successfully", hotel });
    }

    [HttpPost("hotels/{id}/reject")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectHotel(int id, [FromBody] HotelRejectDto dto)
    {
        var hotel = await _adminService.RejectHotelAsync(id, dto.Reason);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(new { message = "Hotel rejected", hotel });
    }

    // Transport Management
    [HttpGet("transports")]
    [ProducesResponseType(typeof(List<NileCareAPI.Models.Transport>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTransports()
    {
        var transports = await _adminService.GetAllTransportsAsync();
        return Ok(transports);
    }

    [HttpGet("transports/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Transport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransport(int id)
    {
        var transport = await _adminService.GetTransportByIdAsync(id);
        if (transport == null)
            return NotFound(new { message = "Transport not found" });

        return Ok(transport);
    }

    [HttpPost("transports")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Transport), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTransport([FromForm] CreateTransportDto dto, [FromForm] IFormFile? image)
    {
        var transport = await _adminService.CreateTransportAsync(dto, image);
        return CreatedAtAction(nameof(GetTransport), new { id = transport.Id }, transport);
    }

    [HttpPut("transports/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Transport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransport(int id, [FromForm] UpdateTransportDto dto, [FromForm] IFormFile? image)
    {
        var transport = await _adminService.UpdateTransportAsync(id, dto, image);
        if (transport == null)
            return NotFound(new { message = "Transport not found" });

        return Ok(transport);
    }

    [HttpDelete("transports/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransport(int id)
    {
        var result = await _adminService.DeleteTransportAsync(id);
        if (!result)
            return NotFound(new { message = "Transport not found" });

        return Ok(new { message = "Transport deleted successfully" });
    }
}
