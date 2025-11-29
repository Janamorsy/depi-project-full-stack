using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[Authorize(Roles = "HotelOwner")]
[ApiController]
[Route("api/hotel-dashboard")]
public class HotelDashboardController : ControllerBase
{
    private readonly IHotelOwnerService _hotelOwnerService;

    public HotelDashboardController(IHotelOwnerService hotelOwnerService)
    {
        _hotelOwnerService = hotelOwnerService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    [HttpGet("stats")]
    [ProducesResponseType(typeof(HotelDashboardStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = await _hotelOwnerService.GetDashboardStatsAsync(GetUserId());
        return Ok(stats);
    }

    [HttpGet("hotels")]
    [ProducesResponseType(typeof(List<NileCareAPI.Models.Hotel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyHotels()
    {
        var hotels = await _hotelOwnerService.GetMyHotelsAsync(GetUserId());
        return Ok(hotels);
    }

    [HttpGet("hotels/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _hotelOwnerService.GetHotelByIdAsync(GetUserId(), id);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(hotel);
    }

    [HttpPost("hotels")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status201Created)]
    public async Task<IActionResult> SubmitHotelRequest([FromForm] HotelRequestDto dto, [FromForm] List<IFormFile>? images)
    {
        var hotel = await _hotelOwnerService.SubmitHotelRequestAsync(GetUserId(), dto, images);
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("hotels/{id}")]
    [ProducesResponseType(typeof(NileCareAPI.Models.Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel(int id, [FromForm] HotelRequestDto dto, [FromForm] List<IFormFile>? images, [FromForm] List<int>? deleteImageIds)
    {
        var hotel = await _hotelOwnerService.UpdateHotelAsync(GetUserId(), id, dto, images, deleteImageIds);
        if (hotel == null)
            return NotFound(new { message = "Hotel not found" });

        return Ok(hotel);
    }

    [HttpDelete("hotels/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await _hotelOwnerService.DeleteHotelAsync(GetUserId(), id);
        if (!result)
            return BadRequest(new { message = "Cannot delete approved hotels or hotel not found" });

        return Ok(new { message = "Hotel request deleted successfully" });
    }

    [HttpGet("bookings")]
    [ProducesResponseType(typeof(List<HotelBookingDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookings([FromQuery] int? hotelId)
    {
        var bookings = await _hotelOwnerService.GetHotelBookingsAsync(GetUserId(), hotelId);
        return Ok(bookings);
    }

    [HttpPut("bookings/{bookingId}/status")]
    [ProducesResponseType(typeof(HotelBookingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBookingStatus(int bookingId, [FromBody] UpdateBookingStatusDto dto)
    {
        var booking = await _hotelOwnerService.UpdateBookingStatusAsync(GetUserId(), bookingId, dto.Status);
        if (booking == null)
            return NotFound(new { message = "Booking not found or you don't have permission to update it" });

        return Ok(booking);
    }

    [HttpGet("booking-counts")]
    [ProducesResponseType(typeof(List<HotelBookingCountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookingCounts()
    {
        var counts = await _hotelOwnerService.GetBookingCountsPerHotelAsync(GetUserId());
        return Ok(counts);
    }
}

public class UpdateBookingStatusDto
{
    public string Status { get; set; } = string.Empty;
}
