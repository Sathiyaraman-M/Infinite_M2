namespace Infinite.Core.Interfaces.Features;

public interface IUserLikesService
{
    Task<IResult<LikeResponse>> IsBlogLiked(string blogId, string userId);

    Task<IResult<LikeResponse>> ToggleBlogLike(string blogId, string userId);

    Task<PaginatedResult<MinifiedBlogResponse>> GetLikedBlogs(int pageNumber, int pageSize, string userId);
    
    Task<IResult<LikeResponse>> IsProjectLiked(string projectId, string userId);

    Task<IResult<LikeResponse>> ToggleProjectLike(string projectId, string userId);

    Task<PaginatedResult<MinifiedProjectResponse>> GetLikedProjects(int pageNumber, int pageSize, string userId);
}