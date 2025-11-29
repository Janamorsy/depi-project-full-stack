using Microsoft.AspNetCore.Mvc;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentWebhookController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public PaymentWebhookController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("stripe")]
    public async Task<IActionResult> HandleStripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        if (string.IsNullOrEmpty(signature))
        {
            return BadRequest("Missing Stripe signature");
        }

        var success = await _stripeService.HandleWebhookEventAsync(json, signature);

        if (!success)
        {
            return BadRequest("Webhook signature verification failed");
        }

        return Ok();
    }
}
