namespace Infinite.Base.Requests;

public class ProjectBlogsRequest
{
    public string Id { get; set; }
    public List<MinifiedBlogResponse> Blogs { get; set; } = new();
}