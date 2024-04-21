namespace Infinite.Base.Responses;

public record FullBlogResponse
{
    public string Id { get; init; }
    public string AuthorId { get; init; }
    public string Title { get; init; }
    public string AuthorName { get; init; }
    public string MarkdownContent { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastEditedDate { get; init; }
    public Visibility Visibility { get; init; }
}