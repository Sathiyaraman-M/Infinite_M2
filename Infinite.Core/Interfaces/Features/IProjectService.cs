namespace Infinite.Core.Interfaces.Features;

public interface IProjectService
{
    Task<IResult<List<ProjectAuthorResponse>>> SearchAuthors(string searchString);
    Task<IResult<List<MinifiedBlogResponse>>> SearchBlogs(GetBlogsRequest request);
    Task<IResult<FullProjectResponse>> GetProject(string id, string userId);
    Task<IResult<List<string>>> GetProjectAuthors(string projectId, string userId);
    Task<IResult<List<ProjectBlogResponse>>> GetProjectBlogs(string projectId, string userId);
    Task<IResult<List<MinifiedProjectResponse>>> GetRecentNProjects(string authorId, string userId, string exclude = null, int n = 4);
    Task<PaginatedResult<MinifiedProjectResponse>> GetAllProjects(int pageNumber, int pageSize, string searchString, string authorId, string userId);
    Task<IResult<string>> SaveProject(EditProjectRequest request, string userId);
    Task<IResult> SaveProjectBlogs(ProjectBlogsRequest request, string userId);
    Task<IResult> DeleteProject(string id, string userId);
}