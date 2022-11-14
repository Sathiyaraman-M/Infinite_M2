namespace Infinite.Base.Entities;

public class UserCurrentSubscription : IEntity<string>
{
    public string Id { get; set; }
    
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public SubscriptionPlan SubscriptionPlan { get; set; }
    
    public DateTime SubscriptionDate { get; set; }
    public SubscriptionBasis SubscriptionBasis { get; set; }
    
    public DateTime ExpiresOn { get; set; }
}