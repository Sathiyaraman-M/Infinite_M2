namespace Infinite.Base.Entities;

public class ProjectLike : IEntity<string>
{
    public string Id { get; set; }
    
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public Project Project { get; set; }
    public string ProjectId { get; set; }
}