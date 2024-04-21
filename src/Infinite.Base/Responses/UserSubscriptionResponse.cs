namespace Infinite.Base.Responses;

public record UserSubscriptionResponse
{
    public DateTime SubscriptionDate { get; init; }
    public SubscriptionPlan SubscriptionPlan { get; init; }
    public SubscriptionBasis SubscriptionBasis { get; init; }
    public DateTime ExpiresOn { get; init; }
}