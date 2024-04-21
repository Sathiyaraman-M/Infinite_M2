namespace Infinite.Base.Responses;

public record MinifiedBlogResponse
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string AuthorName { get; init; }
    public DateTime CreatedDate { get; init; }
}