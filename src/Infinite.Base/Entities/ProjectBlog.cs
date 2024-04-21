namespace Infinite.Base.Entities;

public class ProjectBlog : IEntity<string>
{
    public string Id { get; set; }
    
    public Project Project { get; set; }
    public string ProjectId { get; set; }
    
    public Blog Blog { get; set; }
    public string BlogId { get; set; }
    public int Index { get; set; }
}