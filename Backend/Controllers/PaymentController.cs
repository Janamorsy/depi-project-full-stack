using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;
using NileCareAPI.Services;
using Stripe.Checkout;

namespace NileCareAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly NileCareDbContextV2 _dbContext;
    private readonly IStripeService _stripeService;

    public PaymentController(NileCareDbContextV2 dbContext, IStripeService stripeService)
    {
        _dbContext = dbContext;
        _stripeService = stripeService;
    }

    [HttpPost("appointment/{appointmentId}")]
    [Authorize]
    public async Task<IActionResult> PayForAppointment(int appointmentId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var appointment = await _dbContext.Appointments
            .Include(a => a.DoctorUser)
            .Include(a => a.LegacyDoctor)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.UserId == userId);

        if (appointment == null) return NotFound("Appointment not found");
        if (appointment.PaymentStatus == "Paid") return BadRequest("Appointment is already paid");

        var doctorName = appointment.DoctorUser != null 
            ? $"Dr. {appointment.DoctorUser.FirstName} {appointment.DoctorUser.LastName}"
            : appointment.LegacyDoctor?.Name ?? "Doctor";

        var bookingItem = new BookingItem
        {
            ServiceId = appointment.Id,
            ServiceName = $"Appointment: {appointment.AppointmentType} with {doctorName}",
            UnitPrice = appointment.ConsultationFee,
            Quantity = 1,
            InternalReference = $"APPT-{appointment.Id}"
        };

        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            "/payment/success",
            "/payment/cancel"
        );

        return Ok(sessionUrl);
    }

    [HttpPost("hotel/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> PayForHotelBooking(int bookingId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var booking = await _dbContext.HotelBookings
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null) return NotFound("Hotel booking not found");
        if (booking.PaymentStatus == "Paid") return BadRequest("Booking is already paid");

        var bookingItem = new BookingItem
        {
            ServiceId = booking.Id,
            ServiceName = $"Hotel: {booking.Hotel.Name} ({booking.RoomType}) - {booking.NumberOfNights} nights",
            UnitPrice = booking.TotalPrice,
            Quantity = 1,
            InternalReference = $"HOTEL-{booking.Id}"
        };

        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            "/payment/success",
            "/payment/cancel"
        );

        return Ok(sessionUrl);
    }

    [HttpPost("transport/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> PayForTransportBooking(int bookingId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var booking = await _dbContext.TransportBookings
            .Include(b => b.Transport)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null) return NotFound("Transport booking not found");
        if (booking.PaymentStatus == "Paid") return BadRequest("Booking is already paid");

        var bookingItem = new BookingItem
        {
            ServiceId = booking.Id,
            ServiceName = $"Transport: {booking.Transport.VehicleType} from {booking.PickupLocation} to {booking.DropoffLocation}",
            UnitPrice = booking.TotalPrice,
            Quantity = 1,
            InternalReference = $"TRANS-{booking.Id}"
        };

        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            "/payment/success",
            "/payment/cancel"
        );

        return Ok(sessionUrl);
    }

    [HttpGet("verify/{sessionId}")]
    [Authorize]
    public async Task<IActionResult> VerifyPayment(string sessionId)
    {
        try
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                // Update payment records if webhook hasn't processed yet
                var payments = await _dbContext.Payments
                    .Where(p => p.StripeSessionId == sessionId && p.Status != "Completed")
                    .ToListAsync();

                foreach (var payment in payments)
                {
                    payment.Status = "Completed";
                    payment.StripePaymentIntentId = session.PaymentIntentId ?? "";
                    payment.CompletedAt = DateTime.UtcNow;

                    // Update booking payment status
                    await UpdateBookingPaymentStatusAsync(payment.BookingType, payment.BookingId, "Paid");
                }

                if (payments.Any())
                {
                    await _dbContext.SaveChangesAsync();
                }

                return Ok(new { 
                    status = "paid", 
                    message = "Payment completed successfully",
                    bookings = payments.Select(p => new { p.BookingType, p.BookingId })
                });
            }

            return Ok(new { 
                status = session.PaymentStatus, 
                message = "Payment not yet completed" 
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to verify payment: {ex.Message}" });
        }
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> GetPaymentHistory()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var payments = await _dbContext.Payments
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new
            {
                p.Id,
                p.Amount,
                p.Currency,
                p.Status,
                p.BookingType,
                p.BookingId,
                p.CreatedAt,
                p.CompletedAt
            })
            .ToListAsync();

        return Ok(payments);
    }

    private async Task UpdateBookingPaymentStatusAsync(string bookingType, int bookingId, string status)
    {
        switch (bookingType)
        {
            case "APPT":
                var appointment = await _dbContext.Appointments.FindAsync(bookingId);
                if (appointment != null)
                {
                    appointment.PaymentStatus = status;
                }
                break;
            case "HOTEL":
                var hotelBooking = await _dbContext.HotelBookings.FindAsync(bookingId);
                if (hotelBooking != null)
                {
                    hotelBooking.PaymentStatus = status;
                }
                break;
            case "TRANS":
                var transportBooking = await _dbContext.TransportBookings.FindAsync(bookingId);
                if (transportBooking != null)
                {
                    transportBooking.PaymentStatus = status;
                }
                break;
        }
    }
}
