namespace Infinite.Base.Entities;

public class ActivityPost : IEntity<string>
{
    public string Id { get; set; }
    
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public string Content { get; set; }
    public string Tags { get; set; }
    public DateTime PostDateTime { get; set; }
}