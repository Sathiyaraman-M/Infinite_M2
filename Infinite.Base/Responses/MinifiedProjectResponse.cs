namespace Infinite.Base.Responses;

public record MinifiedProjectResponse
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string Tags { get; init; }
    public bool TeamProject { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public bool Ongoing { get; init; }
    public string Description { get; init; }
}