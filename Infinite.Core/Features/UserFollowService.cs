namespace Infinite.Core.Features;

public class UserFollowService : IUserFollowService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserFollowService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowedPeopleDetails(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            return await _unitOfWork.GetRepository<UserFollow>().Entities
                .Include(x => x.Followed)
                .Include(x => x.FollowedProfileInfo)
                .Where(x => x.FollowerId == userId)
                .Select(x => new AuthorPublicInfoResponse()
                {
                    AboutMe = x.FollowedProfileInfo.AboutMe,
                    Country = x.FollowedProfileInfo.Country,
                    FullName = x.FollowedProfileInfo.FullName,
                    Name = x.Followed.UserName,
                    Status = x.FollowedProfileInfo.Status
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<AuthorPublicInfoResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowersDetails(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            return await _unitOfWork.GetRepository<UserFollow>().Entities
                .Include(x => x.Follower)
                .Include(x => x.FollowerProfileInfo)
                .Where(x => x.FollowerId == userId)
                .Select(x => new AuthorPublicInfoResponse()
                {
                    AboutMe = x.FollowerProfileInfo.AboutMe,
                    Country = x.FollowerProfileInfo.Country,
                    FullName = x.FollowerProfileInfo.FullName,
                    Name = x.Follower.UserName,
                    Status = x.FollowerProfileInfo.Status
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<AuthorPublicInfoResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<IResult<bool>> ToggleUserFollow(string userId, string followedId)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult<FollowStatResponse>> GetFollowStatResponse(string userId)
    {
        throw new NotImplementedException();
    }
}