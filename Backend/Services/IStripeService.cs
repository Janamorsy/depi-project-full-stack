using NileCareAPI.Models;

namespace NileCareAPI.Services;

public interface IStripeService
{
    Task<string> CreateCheckoutSessionAsync(
        string userId,
        List<BookingItem> items,
        string successUrl,
        string cancelUrl);

    Task<bool> HandleWebhookEventAsync(string json, string stripeSignature);
}