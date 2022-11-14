namespace Infinite.Base.Requests;

public class EditBlogRequest
{
    public string BlogId { get; set; }
    public string DraftId { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public Visibility Visibility { get; set; }
    public string Markdown { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastSaveTime { get; set; }
    public string ProjectId { get; set; }
}