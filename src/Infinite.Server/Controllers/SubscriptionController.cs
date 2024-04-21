using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/subscription")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService) => _subscriptionService = subscriptionService;

    [HttpGet]
    public async Task<IActionResult> GetSubscriptionDetails()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _subscriptionService.GetCurrentSubscription(userId));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSubscriptionDetails(UpdateSubscriptionRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _subscriptionService.UpdateSubscription(request, userId));
    }
}