using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/subscription")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSubscriptionDetails()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await subscriptionService.GetCurrentSubscription(userId));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSubscriptionDetails(UpdateSubscriptionRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await subscriptionService.UpdateSubscription(request, userId));
    }
}