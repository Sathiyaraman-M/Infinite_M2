namespace Infinite.Core.Interfaces.Features;

public interface IUserFollowService
{
    Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowedPeopleDetails(int pageNumber, int pageSize, string searchString, string userId);
    Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowersDetails(int pageNumber, int pageSize, string searchString, string userId);
    Task<IResult<bool>> ToggleUserFollow(string userId, string followedId);
    Task<IResult<FollowStatResponse>> GetFollowStatResponse(string userId);
}