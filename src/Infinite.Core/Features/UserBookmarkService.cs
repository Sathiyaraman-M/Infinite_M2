namespace Infinite.Core.Features;

public class UserBookmarkService(IUnitOfWork unitOfWork) : IUserBookmarkService
{
    public async Task<IResult<bool>> IsBlogBookmarked(string blogId, string userId)
    {
        try
        {
            var bookmark = await unitOfWork.GetRepository<BlogBookmark>().Entities
                .FirstOrDefaultAsync(x => x.BlogId == blogId && x.UserId == userId);
            return await Result<bool>.SuccessAsync(bookmark != null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<bool>> ToggleBlogBookmark(string blogId, string userId)
    {
        try
        {
            var bookmark = await unitOfWork.GetRepository<BlogBookmark>().Entities
                .FirstOrDefaultAsync(x => x.BlogId == blogId && x.UserId == userId);
            if (bookmark == null)
            {
                var newBookmark = new BlogBookmark()
                {
                    Id = Guid.NewGuid().ToString(),
                    BlogId = blogId,
                    UserId = userId
                };
                await unitOfWork.GetRepository<BlogBookmark>().AddAsync(newBookmark);
            }
            else
            {
                await unitOfWork.GetRepository<BlogBookmark>().DeleteAsync(bookmark);
            }
            await unitOfWork.Commit();
            return await Result<bool>.SuccessAsync(bookmark == null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetBookmarkedBlogs(int pageNumber, int pageSize, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<BlogBookmark>()
                .Entities
                .Include(x => x.Blog)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Blog.CreatedDate)
                .Select(x => new MinifiedBlogResponse()
                {
                    AuthorName = x.Blog.AuthorName,
                    CreatedDate = x.Blog.CreatedDate,
                    Id = x.BlogId,
                    Title = x.Blog.Title
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<MinifiedBlogResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<IResult<bool>> IsProjectBookmarked(string projectId, string userId)
    {
        try
        {
            var bookmark = await unitOfWork.GetRepository<ProjectBookmark>().Entities
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
            return await Result<bool>.SuccessAsync(bookmark != null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<bool>> ToggleProjectBookmark(string projectId, string userId)
    {
        try
        {
            var bookmark = await unitOfWork.GetRepository<ProjectBookmark>().Entities
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
            if (bookmark == null)
            {
                var newBookmark = new ProjectBookmark()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = projectId,
                    UserId = userId
                };
                await unitOfWork.GetRepository<ProjectBookmark>().AddAsync(newBookmark);
            }
            else
            {
                await unitOfWork.GetRepository<ProjectBookmark>().DeleteAsync(bookmark);
            }
            await unitOfWork.Commit();
            return await Result<bool>.SuccessAsync(bookmark == null);
        }
        catch (Exception e)
        {
            return await Result<bool>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetBookmarkedProjects(int pageNumber, int pageSize, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<ProjectBookmark>()
                .Entities
                .Include(x => x.Project)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Project.CreatedDate)
                .Select(x => new MinifiedProjectResponse()
                {
                    Id = x.ProjectId,
                    TeamProject = x.Project.TeamProject,
                    Description = x.Project.Description,
                    EndDate = x.Project.EndDate,
                    StartDate = x.Project.StartDate,
                    Tags = x.Project.Tags,
                    Title = x.Project.Title,
                    Ongoing = x.Project.Ongoing
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<MinifiedProjectResponse>.Failure(new List<string>() { e.Message });
        }
    }
}