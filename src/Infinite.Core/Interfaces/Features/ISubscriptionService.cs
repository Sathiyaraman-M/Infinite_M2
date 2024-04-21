namespace Infinite.Core.Interfaces.Features;

public interface ISubscriptionService
{
    Task<IResult<UserSubscriptionResponse>> GetCurrentSubscription(string userId);

    Task<IResult> UpdateSubscription(UpdateSubscriptionRequest request, string userId);
}