namespace Infinite.Core.Interfaces.Features;

public interface IBlogService
{
    Task<IResult<FullBlogResponse>> GetFullBlog(string id, string userId);
    Task<IResult<FullBlogDraftResponse>> GetFullBlogDraft(string id, string userId);
    Task<IResult<List<MinifiedBlogResponse>>> GetRecentNBlogs(string authorId, string userId, string exclude = null, int n = 4);
    Task<IResult<List<MinifiedBlogDraftResponse>>> GetRecentNDrafts(string userId, int n = 4);
    Task<PaginatedResult<MinifiedBlogResponse>> GetAllBlogs(int pageNumber, int pageSize, string searchString, string authorId, string userId);
    Task<PaginatedResult<MinifiedBlogDraftResponse>> GetAllDrafts(int pageNumber, int pageSize, string searchString, string userId);
    Task<IResult> PublishAsync(EditBlogRequest request, string userId);
    Task<IResult> SaveToDraftsAsync(EditBlogRequest request, string userId);
    Task<IResult> DeleteBlogAsync(string id, string userId);
    Task<IResult> DiscardDraftAsync(string id, string userId);
}