namespace Infinite.Core.Interfaces.Features;

public interface IUserBookmarkService
{
    Task<IResult<bool>> IsBlogBookmarked(string blogId, string userId);
    
    Task<IResult<bool>> ToggleBlogBookmark(string blogId, string userId);

    Task<PaginatedResult<MinifiedBlogResponse>> GetBookmarkedBlogs(int pageNumber, int pageSize, string userId);
    
    Task<IResult<bool>> IsProjectBookmarked(string projectId, string userId);
    
    Task<IResult<bool>> ToggleProjectBookmark(string projectId, string userId);

    Task<PaginatedResult<MinifiedProjectResponse>> GetBookmarkedProjects(int pageNumber, int pageSize, string userId);
}