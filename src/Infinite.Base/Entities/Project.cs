namespace Infinite.Base.Entities;

public class Project : IEntity<string>
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string ElaborationObjectives { get; set; }
    
    public string Requirements { get; set; } 
    
    public string Tags { get; set; }
    public bool TeamProject { get; set; }
    public List<UserProject> UserProjects { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime LastEditedDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Ongoing { get; set; }
    
    public ProjectComplexity Complexity { get; set; }
    
    public ProjectCapitalSourceType CapitalSourceType { get; set; }
    
    public ProjectObjective Objective { get; set; }
    
    public ProjectType Type { get; set; }
    
    public Visibility Visibility { get; set; }
    
    public string Abstract { get; set; }

    public List<ProjectBlog> Blogs { get; set; } = new();
}