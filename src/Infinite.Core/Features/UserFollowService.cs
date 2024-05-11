namespace Infinite.Core.Features;

public class UserFollowService(IUnitOfWork unitOfWork) : IUserFollowService
{
    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowedPeopleDetails(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<UserFollow>().Entities
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

    public async Task<IResult<List<AuthorPublicInfoResponse>>> GetAllFollowedPeopleDetails(string userId)
    {
        try
        {
            var followed = await unitOfWork.GetRepository<UserFollow>().Entities
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
                .ToListAsync();
            return await Result<List<AuthorPublicInfoResponse>>.SuccessAsync(followed);
        }
        catch (Exception e)
        {
            return await Result<List<AuthorPublicInfoResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowersDetails(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<UserFollow>().Entities
                .Include(x => x.Follower)
                .Include(x => x.FollowerProfileInfo)
                .Where(x => x.FollowedId == userId)
                .Select(x => new AuthorPublicInfoResponse()
                {
                    Id = x.FollowerProfileInfo.UserId,
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

    public async Task<IResult<List<AuthorPublicInfoResponse>>> GetAllFollowersDetails(string userId)
    {
        try
        {
            var followed = await unitOfWork.GetRepository<UserFollow>().Entities
                .Include(x => x.Follower)
                .Include(x => x.FollowerProfileInfo)
                .Where(x => x.FollowedId == userId)
                .Select(x => new AuthorPublicInfoResponse()
                {
                    Id = x.FollowerProfileInfo.UserId,
                    AboutMe = x.FollowerProfileInfo.AboutMe,
                    Country = x.FollowerProfileInfo.Country,
                    FullName = x.FollowerProfileInfo.FullName,
                    Name = x.Follower.UserName,
                    Status = x.FollowerProfileInfo.Status
                })
                .ToListAsync();
            return await Result<List<AuthorPublicInfoResponse>>.SuccessAsync(followed);
        }
        catch (Exception e)
        {
            return await Result<List<AuthorPublicInfoResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<bool>> IsUserFollowed(string userId, string authorId)
    {
        try
        {
            var userFollow = await unitOfWork.GetRepository<UserFollow>().Entities
                .FirstOrDefaultAsync(x => x.FollowedId == authorId && x.FollowerId == userId);
            return await Result<bool>.SuccessAsync(userFollow != null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<bool>> ToggleUserFollow(string userId, string followedId)
    {
        try
        {
            if (userId == followedId)
                throw new Exception("One cannot follow himself/herself");
            var userFollow = await unitOfWork.GetRepository<UserFollow>().Entities
                .Where(x => x.FollowedId == followedId && x.FollowerId == userId)
                .FirstOrDefaultAsync();
            if (userFollow == null)
            {
                var followerProfile = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                    .FirstAsync(x => x.UserId == userId);
                var followedProfile = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                    .FirstAsync(x => x.UserId == followedId);
                var newUserFollow = new UserFollow()
                {
                    Id = Guid.NewGuid().ToString(),
                    FollowedId = followedId,
                    FollowedProfileInfoId = followedProfile.Id,
                    FollowerId = userId,
                    FollowerProfileInfoId = followerProfile.Id,
                };
                await unitOfWork.GetRepository<UserFollow>().AddAsync(newUserFollow);
            }
            else
            {
                await unitOfWork.GetRepository<UserFollow>().DeleteAsync(userFollow);
            }
            await unitOfWork.Commit();
            return await Result<bool>.SuccessAsync(userFollow == null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<FollowStatResponse>> GetFollowStatResponse(string userId)
    {
        try
        {
            var userProfileInfo = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                .Include(x => x.User)
                .FirstAsync(x => x.UserId == userId);
            var followed = await unitOfWork.GetRepository<UserFollow>().Entities
                .CountAsync(x => x.FollowerId == userId);
            var followers = await unitOfWork.GetRepository<UserFollow>().Entities
                .CountAsync(x => x.FollowedId == userId);
            var response = new FollowStatResponse(userId, userProfileInfo.User.UserName, userProfileInfo.FullName,
                followers, followed);
            return await Result<FollowStatResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<FollowStatResponse>.FailAsync(e.Message);
        }
    }
}