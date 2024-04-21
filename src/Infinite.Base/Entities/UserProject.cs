namespace Infinite.Base.Entities;

public class UserProject : IEntity<string>
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string ProjectId { get; set; }
    public Project Project { get; set; }
}