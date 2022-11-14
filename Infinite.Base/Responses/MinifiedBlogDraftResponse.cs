namespace Infinite.Base.Responses;

public record MinifiedBlogDraftResponse
{
    public string Id { get; init; }
    public string BlogId { get; init; } = Guid.Empty.ToString();
    public DateTime SaveDateTime { get; init; }
    public string Title { get; init; }
    public string AuthorName { get; init; }
}