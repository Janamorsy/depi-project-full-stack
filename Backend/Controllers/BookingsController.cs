using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("appointments")]
    [ProducesResponseType(typeof(List<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointments()
    {
        var appointments = await _bookingService.GetUserAppointmentsAsync();
        return Ok(appointments);
    }



    [HttpPut("appointments/{id}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointment(int id, [FromBody] BookAppointmentDto updateDto)
    {
        var appointment = await _bookingService.UpdateAppointmentAsync(id, updateDto);
        if (appointment == null)
            return NotFound("Appointment not found or you don't have permission to update it.");
        return Ok(appointment);
    }



    [HttpDelete("appointments/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var result = await _bookingService.DeleteAppointmentAsync(id);
        if (!result)
            return NotFound("Appointment not found or you don't have permission to delete it.");
        return Ok(new { message = "Appointment deleted successfully" });
    }


    [HttpGet("hotels")]
    [ProducesResponseType(typeof(List<HotelBookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotelBookings()
    {
        var bookings = await _bookingService.GetUserHotelBookingsAsync();
        return Ok(bookings);
    }



    [HttpPut("hotels/{id}")]
    [ProducesResponseType(typeof(HotelBookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotelBooking(int id, [FromBody] CreateHotelBookingDto updateDto)
    {
        var booking = await _bookingService.UpdateHotelBookingAsync(id, updateDto);
        if (booking == null)
            return NotFound("Hotel booking not found or you don't have permission to update it.");
        return Ok(booking);
    }



    [HttpDelete("hotels/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotelBooking(int id)
    {
        var result = await _bookingService.DeleteHotelBookingAsync(id);
        if (!result)
            return NotFound("Hotel booking not found or you don't have permission to delete it.");
        return Ok(new { message = "Hotel booking deleted successfully" });
    }




    [HttpGet("transport")]
    [ProducesResponseType(typeof(List<TransportBookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransportBookings()
    {
        var bookings = await _bookingService.GetUserTransportBookingsAsync();
        return Ok(bookings);
    }

    [HttpPut("transport/{id}")]
    [ProducesResponseType(typeof(TransportBookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransportBooking(int id, [FromBody] CreateTransportBookingDto updateDto)
    {
        var booking = await _bookingService.UpdateTransportBookingAsync(id, updateDto);
        if (booking == null)
            return NotFound("Transport booking not found or you don't have permission to update it.");
        return Ok(booking);
    }



    [HttpDelete("transport/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransportBooking(int id)
    {
        var result = await _bookingService.DeleteTransportBookingAsync(id);
        if (!result)
            return NotFound("Transport booking not found or you don't have permission to delete it.");
        return Ok(new { message = "Transport booking deleted successfully" });
    }
    //for paymenttttt
    [HttpPost("hotels")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookHotel([FromBody] CreateHotelBookingDto bookingDto)
    {
        var successUrl = "/payment/success";
        var cancelUrl = "/payment/cancel";

        try
        {
            var stripeUrl = await _bookingService.BookHotelAndCreatePaymentSessionAsync(bookingDto, successUrl, cancelUrl);
            return Ok(stripeUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to book hotel or initiate payment: {ex.Message}" });
        }
    }
    [HttpPost("appointments/payment")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookAppointmentWithPayment([FromBody] BookAppointmentDto bookingDto)
    {
        var successUrl = "/payment/success";
        var cancelUrl = "/payment/cancel";

        try
        {
            var stripeUrl = await _bookingService.BookAppointmentAndCreatePaymentSessionAsync(bookingDto, successUrl, cancelUrl);
            return Ok(stripeUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to book appointment or initiate payment: {ex.Message}" });
        }
    }
    [HttpPost("transport/payment")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookTransportWithPayment([FromBody] CreateTransportBookingDto bookingDto)
    {
        var successUrl = "/payment/success";
        var cancelUrl = "/payment/cancel";

        try
        {
            var stripeUrl = await _bookingService.BookTransportAndCreatePaymentSessionAsync(bookingDto, successUrl, cancelUrl);
            return Ok(stripeUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to book transport or initiate payment: {ex.Message}" });
        }
    }


}


