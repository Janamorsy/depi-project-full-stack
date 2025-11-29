using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelRepository _hotelRepository;

    public HotelsController(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    private static HotelWithRoomTypesDto MapToHotelWithRoomTypes(Hotel hotel)
    {
        return new HotelWithRoomTypesDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            City = hotel.City,
            Address = hotel.Address,
            PricePerNight = hotel.PricePerNight,
            Rating = hotel.Rating,
            WheelchairAccessible = hotel.WheelchairAccessible,
            RollInShower = hotel.RollInShower,
            ElevatorAccess = hotel.ElevatorAccess,
            GrabBars = hotel.GrabBars,
            Amenities = hotel.Amenities,
            ImageUrl = hotel.ImageUrl,
            Description = hotel.Description,
            RoomTypes = new List<RoomTypeDto>
            {
                new() { Name = "Standard", PricePerNight = hotel.StandardRoomPrice, MaxGuests = hotel.StandardRoomMaxGuests },
                new() { Name = "Deluxe", PricePerNight = hotel.DeluxeRoomPrice, MaxGuests = hotel.DeluxeRoomMaxGuests },
                new() { Name = "Suite", PricePerNight = hotel.SuiteRoomPrice, MaxGuests = hotel.SuiteRoomMaxGuests },
                new() { Name = "Family", PricePerNight = hotel.FamilyRoomPrice, MaxGuests = hotel.FamilyRoomMaxGuests }
            },
            Images = hotel.Images?.Select(i => new HotelImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                DisplayOrder = i.DisplayOrder
            }).ToList() ?? new List<HotelImageDto>()
        };
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<HotelWithRoomTypesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHotels([FromQuery] string? city, [FromQuery] bool? wheelchairAccessible)
    {
        var hotels = await _hotelRepository.SearchAsync(city, wheelchairAccessible);
        var hotelsWithRoomTypes = hotels.Select(MapToHotelWithRoomTypes).ToList();
        return Ok(hotelsWithRoomTypes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HotelWithRoomTypesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
            return NotFound();

        return Ok(MapToHotelWithRoomTypes(hotel));
    }

    [HttpGet("{id}/room-types")]
    [ProducesResponseType(typeof(List<RoomTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoomTypes(int id)
    {
        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
            return NotFound();

        var roomTypes = new List<RoomTypeDto>
        {
            new() { Name = "Standard", PricePerNight = hotel.StandardRoomPrice, MaxGuests = hotel.StandardRoomMaxGuests },
            new() { Name = "Deluxe", PricePerNight = hotel.DeluxeRoomPrice, MaxGuests = hotel.DeluxeRoomMaxGuests },
            new() { Name = "Suite", PricePerNight = hotel.SuiteRoomPrice, MaxGuests = hotel.SuiteRoomMaxGuests },
            new() { Name = "Family", PricePerNight = hotel.FamilyRoomPrice, MaxGuests = hotel.FamilyRoomMaxGuests }
        };

        return Ok(roomTypes);
    }
}


