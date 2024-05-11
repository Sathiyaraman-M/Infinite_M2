namespace Infinite.Core.Features;

public class UserLikesService(IUnitOfWork unitOfWork) : IUserLikesService
{
    public async Task<IResult<LikeResponse>> IsBlogLiked(string blogId, string userId)
    {
        try
        {
            var like = await unitOfWork.GetRepository<BlogLike>().Entities
                .FirstOrDefaultAsync(x => x.BlogId == blogId && x.UserId == userId);
            var count = await unitOfWork.GetRepository<BlogLike>().CountAsync(x => x.BlogId == blogId);
            return await Result<LikeResponse>.SuccessAsync(new LikeResponse(count, like != null));
        }
        catch (Exception e)
        {
            return await Result<LikeResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<LikeResponse>> ToggleBlogLike(string blogId, string userId)
    {
        try
        {
            var like = await unitOfWork.GetRepository<BlogLike>().Entities
                .FirstOrDefaultAsync(x => x.BlogId == blogId && x.UserId == userId);
            if (like == null)
            {
                var newBlogLike = new BlogLike()
                {
                    Id = Guid.NewGuid().ToString(),
                    BlogId = blogId,
                    UserId = userId
                };
                await unitOfWork.GetRepository<BlogLike>().AddAsync(newBlogLike);
            }
            else
            {
                await unitOfWork.GetRepository<BlogLike>().DeleteAsync(like);
            }
            await unitOfWork.Commit();
            var count = await unitOfWork.GetRepository<BlogLike>().CountAsync(x => x.BlogId == blogId);
            return await Result<LikeResponse>.SuccessAsync(new LikeResponse(count, like == null));
        }
        catch (Exception e)
        {
            return await Result<LikeResponse>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetLikedBlogs(int pageNumber, int pageSize, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<BlogLike>()
                .Entities
                .Include(x => x.Blog)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Blog.CreatedDate)
                .Select(x => new MinifiedBlogResponse
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

    public async Task<IResult<LikeResponse>> IsProjectLiked(string projectId, string userId)
    {
        try
        {
            var like = await unitOfWork.GetRepository<ProjectLike>().Entities
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
            var count = await unitOfWork.GetRepository<ProjectLike>().CountAsync(x => x.ProjectId == projectId);
            return await Result<LikeResponse>.SuccessAsync(new LikeResponse(count, like != null));
        }
        catch (Exception e)
        {
            return await Result<LikeResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<LikeResponse>> ToggleProjectLike(string projectId, string userId)
    {
        try
        {
            var like = await unitOfWork.GetRepository<ProjectLike>().Entities
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
            if (like == null)
            {
                var newBlogLike = new ProjectLike()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProjectId = projectId
                };
                await unitOfWork.GetRepository<ProjectLike>().AddAsync(newBlogLike);
            }
            else
            {
                await unitOfWork.GetRepository<ProjectLike>().DeleteAsync(like);
            }
            await unitOfWork.Commit();
            var count = await unitOfWork.GetRepository<ProjectLike>().CountAsync(x => x.ProjectId == projectId);
            return await Result<LikeResponse>.SuccessAsync(new LikeResponse(count, like == null));
        }
        catch (Exception e)
        {
            return await Result<LikeResponse>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetLikedProjects(int pageNumber, int pageSize, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<ProjectLike>()
                .Entities
                .Include(x => x.Project)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Project.CreatedDate)
                .Select(x => new MinifiedProjectResponse
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