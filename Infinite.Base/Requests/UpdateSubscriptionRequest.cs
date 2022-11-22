namespace Infinite.Base.Requests;

public class UpdateSubscriptionRequest
{
    public SubscriptionPlan SubscriptionPlan { get; set; }
    
    public SubscriptionBasis SubscriptionBasis { get; set; }
}