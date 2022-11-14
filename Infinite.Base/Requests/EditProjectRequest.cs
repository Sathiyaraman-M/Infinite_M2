namespace Infinite.Base.Requests;

public class EditProjectRequest
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tags { get; set; }
    public bool TeamProject { get; set; }
    public List<ProjectAuthorResponse> Authors { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime LastEditedDate { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-2);
    public DateTime? EndDate { get; set; } = DateTime.Today;
    public bool Ongoing { get; set; }

    public Visibility Visibility { get; set; }
    
    public string Abstract { get; set; }
}