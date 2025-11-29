using Microsoft.Extensions.Configuration;
using NileCareAPI.Data;
using NileCareAPI.Models;
using Stripe;
using Stripe.Checkout;

namespace NileCareAPI.Services;

public class StripeService : IStripeService
{
    private readonly string _domain;
    private readonly string _webhookSecret;
    private readonly NileCareDbContextV2 _dbContext;

    public StripeService(IConfiguration configuration, NileCareDbContextV2 dbContext)
    {
        StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"]
                                     ?? throw new ArgumentNullException("StripeSettings:SecretKey not found in configuration.");

        _domain = configuration["StripeSettings:Domain"]
                  ?? throw new ArgumentNullException("StripeSettings:Domain not found in configuration.");

        _webhookSecret = configuration["StripeSettings:WebhookSecret"] ?? "";
        _dbContext = dbContext;
    }

    public async Task<string> CreateCheckoutSessionAsync(
        string userId,
        List<BookingItem> items,
        string successUrl,
        string cancelUrl)
    {
        if (items == null || !items.Any())
        {
            throw new ArgumentException("No chargeable items provided for payment.");
        }

        var lineItems = items.Select(item => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(item.UnitPrice * 100),
                Currency = "egp",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.ServiceName,
                    Description = item.InternalReference
                },
            },
            Quantity = item.Quantity,
        }).ToList();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = _domain.TrimEnd('/') + successUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = _domain.TrimEnd('/') + cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId },
                { "itemReferences", string.Join(",", items.Select(i => i.InternalReference)) }
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        // Create payment record in database
        foreach (var item in items)
        {
            var (bookingType, bookingId) = ParseInternalReference(item.InternalReference);
            var payment = new Payment
            {
                UserId = userId,
                StripeSessionId = session.Id,
                Amount = item.UnitPrice * item.Quantity,
                Currency = "egp",
                Status = "Pending",
                BookingType = bookingType,
                BookingId = bookingId,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Payments.Add(payment);
        }
        await _dbContext.SaveChangesAsync();

        return session.Url;
    }

    public async Task<bool> HandleWebhookEventAsync(string json, string stripeSignature)
    {
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);
        }
        catch (StripeException)
        {
            return false;
        }

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Session;
            if (session != null)
            {
                await HandleSuccessfulPaymentAsync(session);
            }
        }
        else if (stripeEvent.Type == "checkout.session.expired")
        {
            var session = stripeEvent.Data.Object as Session;
            if (session != null)
            {
                await HandleExpiredPaymentAsync(session);
            }
        }

        return true;
    }

    private async Task HandleSuccessfulPaymentAsync(Session session)
    {
        var payments = _dbContext.Payments
            .Where(p => p.StripeSessionId == session.Id)
            .ToList();

        foreach (var payment in payments)
        {
            payment.Status = "Completed";
            payment.StripePaymentIntentId = session.PaymentIntentId ?? "";
            payment.CompletedAt = DateTime.UtcNow;

            // Update the booking's payment status
            await UpdateBookingPaymentStatusAsync(payment.BookingType, payment.BookingId, "Paid");
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task HandleExpiredPaymentAsync(Session session)
    {
        var payments = _dbContext.Payments
            .Where(p => p.StripeSessionId == session.Id)
            .ToList();

        foreach (var payment in payments)
        {
            payment.Status = "Failed";
        }

        await _dbContext.SaveChangesAsync();
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

    private (string bookingType, int bookingId) ParseInternalReference(string reference)
    {
        var parts = reference.Split('-');
        if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
        {
            return (parts[0], id);
        }
        return ("UNKNOWN", 0);
    }
}