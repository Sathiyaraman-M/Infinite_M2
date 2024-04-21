namespace Infinite.Base.Responses;

public record FullProjectResponse
{
    public string Id { get; init; }
    
    public string Title { get; init; }
    public string Description { get; init; }
    
    public string ElaborationObjectives { get; set; }
    
    public string Requirements { get; set; }
    public string Tags { get; init; }
    public bool TeamProject { get; init; }
    public List<ProjectAuthorResponse> Authors { get; init; } = new();
    public DateTime CreatedDate { get; init; }
    public DateTime LastEditedDate { get; init; }

    public Visibility Visibility { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public bool Ongoing { get; init; }
    
    public ProjectComplexity Complexity { get; set; }
    
    public ProjectCapitalSourceType CapitalSourceType { get; set; }
    
    public ProjectObjective Objective { get; set; }
    
    public ProjectType Type { get; set; }
    
    public string Abstract { get; init; }

    public List<ProjectBlogResponse> Blogs { get; init; } = new();
}

public record ProjectAuthorResponse(string Id, string Name);
public record ProjectBlogResponse(string Id, string Title, int Index, string AuthorId, string AuthorName, DateTime CreatedDate);