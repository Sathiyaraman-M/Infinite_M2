namespace Infinite.Base.Entities;

public class BlogLike : IEntity<string>
{
    public string Id { get; set; }
    
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public Blog Blog { get; set; }
    public string BlogId { get; set; }
}