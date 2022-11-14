using Infinite.Core.Specifications;

namespace Infinite.Core.Features;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IResult<List<ProjectAuthorResponse>>> SearchAuthors(string searchString)
    {
        try
        {
            var authors = await _unitOfWork.GetRepository<AppUser>().Entities
                .Specify(new AuthorSearchFilterSpecification(searchString))
                .Select(x => new ProjectAuthorResponse(x.Id, x.UserName))
                .Take(5)
                .ToListAsync();
            return await Result<List<ProjectAuthorResponse>>.SuccessAsync(authors);
        }
        catch (Exception e)
        {
            return await Result<List<ProjectAuthorResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<MinifiedBlogResponse>>> SearchBlogs(GetBlogsRequest request)
    {
        try
        {
            var blogs = await _unitOfWork.GetRepository<Blog>().Entities
                .Where(x => x.Visibility != Visibility.Private && request.AuthorIds.Contains(x.UserId))
                .Specify(new BlogSearchFilterSpecification(request.SearchString))
                .Take(5)
                .Select(x => new MinifiedBlogResponse()
                {
                    AuthorName = x.AuthorName,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    Title = x.Title
                })
                .ToListAsync();
            return await Result<List<MinifiedBlogResponse>>.SuccessAsync(blogs);
        }
        catch (Exception e)
        {
            return await Result<List<MinifiedBlogResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<FullProjectResponse>> GetProject(string id, string userId)
    {
        try
        {
            var project = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.User)
                .Where(x => x.Id == id)
                .Where(x => x.Visibility != Visibility.Private
                            || (x.Visibility == Visibility.Private && x.UserProjects.Any(y => y.UserId == userId)))
                .Select(x => new FullProjectResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Abstract = x.Abstract,
                    TeamProject = x.TeamProject,
                    Tags = x.Tags,
                    Authors = x.UserProjects.Select(y => new ProjectAuthorResponse(y.UserId, y.User.UserName)).ToList(),
                    Visibility = x.Visibility,
                    CreatedDate = x.CreatedDate,
                    LastEditedDate = x.LastEditedDate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Ongoing = x.Ongoing
                })
                .FirstOrDefaultAsync();
            return await Result<FullProjectResponse>.SuccessAsync(project);
        }
        catch (Exception e)
        {
            return await Result<FullProjectResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<string>>> GetProjectAuthors(string projectId, string userId)
    {
        try
        {
            var authors = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.User)
                .Where(x => (x.Visibility == Visibility.Private && x.UserProjects.Any(y => y.UserId == userId))
                            || x.Visibility != Visibility.Private)
                .Where(x => x.Id == projectId)
                .SelectMany(x => x.UserProjects.Select(y => y.UserId))
                .ToListAsync();
            return await Result<List<string>>.SuccessAsync(authors);
        }
        catch (Exception e)
        {
            return await Result<List<string>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<ProjectBlogResponse>>> GetProjectBlogs(string projectId, string userId)
    {
        try
        {
            var authors = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.User)
                .Include(x => x.Blogs)
                .ThenInclude(x => x.Blog)
                .Where(x => (x.Visibility == Visibility.Private && x.UserProjects.Any(y => y.UserId == userId))
                            || x.Visibility != Visibility.Private)
                .Where(x => x.Id == projectId)
                .SelectMany(x => x.Blogs.Select(y => 
                    new ProjectBlogResponse(y.Id, y.Blog.Title, y.Index, y.Blog.UserId,y.Blog.AuthorName, y.Blog.CreatedDate)))
                .ToListAsync();
            return await Result<List<ProjectBlogResponse>>.SuccessAsync(authors);
        }
        catch (Exception e)
        {
            return await Result<List<ProjectBlogResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<MinifiedProjectResponse>>> GetRecentNProjects(string authorId, string userId, string exclude = null, int n = 4)
    {
        try
        {
            var projects = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .Where(x => x.UserProjects.Any(y => y.UserId == authorId))
                .WhereIf(!string.IsNullOrEmpty(exclude), x => x.Id != exclude)
                .WhereIf(authorId != userId, x => x.Visibility == Visibility.Public)
                .OrderByDescending(x => x.CreatedDate)
                .Take(n)
                .Select(x => new MinifiedProjectResponse
                {
                    Id = x.Id,
                    TeamProject = x.TeamProject,
                    Description = x.Description,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    Tags = x.Tags,
                    Title = x.Title,
                    Ongoing = x.Ongoing
                })
                .ToListAsync();
            return await Result<List<MinifiedProjectResponse>>.SuccessAsync(projects);
        }
        catch (Exception e)
        {
            return await Result<List<MinifiedProjectResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetAllProjects(int pageNumber, int pageSize, string searchString, string authorId, string userId)
    {
        try
        {
            return await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .Where(x => x.UserProjects.Any(y => y.UserId == authorId))
                .WhereIf(authorId != userId, x => x.Visibility == Visibility.Public)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new MinifiedProjectResponse
                {
                    Id = x.Id,
                    TeamProject = x.TeamProject,
                    Description = x.Description,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    Tags = x.Tags,
                    Title = x.Title,
                    Ongoing = x.Ongoing
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<MinifiedProjectResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<IResult<string>> SaveProject(EditProjectRequest request, string userId)
    {
        try
        {
            //Perform validation checks
            if (string.IsNullOrEmpty(request.Title))
                throw new Exception("Title cannot be empty");
            if (string.IsNullOrEmpty(request.Description))
                throw new Exception("Description cannot be empty");
            if (request.Authors.All(x => x.Id != userId))
                throw new Exception("You cannot add a project that you didn't author");

            //Check for valid GUID and decide add/edit operation
            var isGuid = Guid.TryParse(request.Id, out var id);
            if (isGuid && id == Guid.Empty)
            {
                //Create a new GUID for the new project
                id = Guid.NewGuid();

                //Create a Project object
                var project = new Project
                {
                    Id = id.ToString(),
                    Abstract = request.Abstract,
                    CreatedDate = DateTime.Now,
                    Description = request.Description,
                    LastEditedDate = request.LastEditedDate,
                    Tags = request.Tags,
                    Title = request.Title,
                    TeamProject = request.TeamProject,
                    Visibility = request.Visibility,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Ongoing = request.Ongoing
                };

                //Add the authors to the project object 
                foreach (var author in request.Authors)
                {
                    await _unitOfWork.GetRepository<UserProject>().AddAsync(new UserProject
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = id.ToString(),
                        UserId = author.Id
                    });
                }

                //Add the project object
                await _unitOfWork.GetRepository<Project>().AddAsync(project);

                //Save changes in database
                await _unitOfWork.Commit();
                return await Result<string>.SuccessAsync(id.ToString(), "Added successfully");
            }

            //Throw exception if invalid Id found
            if (string.IsNullOrEmpty(request.Id) || !isGuid)
                throw new Exception("Invalid Request");

            //Get the existing project object
            var existingProject = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .FirstAsync(x => x.Id == id.ToString());

            //Update the properties
            existingProject.Abstract = request.Abstract;
            existingProject.LastEditedDate = DateTime.Now;
            existingProject.Description = request.Description;
            existingProject.Tags = request.Tags;
            existingProject.Title = request.Title;
            existingProject.TeamProject = request.TeamProject;
            existingProject.Visibility = request.Visibility;
            existingProject.StartDate = request.StartDate;
            existingProject.EndDate = request.EndDate;
            existingProject.Ongoing = request.Ongoing;

            //Remove all author objects
            foreach (var author in existingProject.UserProjects)
            {
                await _unitOfWork.GetRepository<UserProject>().DeleteAsync(author);
            }

            //Add all the updated author objects
            foreach (var author in request.Authors)
            {
                //existingProject.UserProjects.Add(userProject);
                await _unitOfWork.GetRepository<UserProject>().AddAsync(new UserProject
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = id.ToString(),
                    UserId = author.Id
                });
            }

            //Update the existing project
            await _unitOfWork.GetRepository<Project>().UpdateAsync(existingProject, id.ToString());

            //Save changes in database
            await _unitOfWork.Commit();
            return await Result<string>.SuccessAsync(id.ToString(), "Project updated successfully");
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> SaveProjectBlogs(ProjectBlogsRequest request, string userId)
    {
        try
        {
            var project = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .Include(x => x.Blogs)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (project == null)
                throw new Exception("Project not found!");
            if (project.UserProjects.All(x => x.UserId != userId))
                throw new Exception("You cannot edit a project that you aren't part of!");
            foreach (var projectBlog in project.Blogs)
            {
                await _unitOfWork.GetRepository<ProjectBlog>().DeleteAsync(projectBlog);
            }
            foreach (var blog in request.Blogs)
            {
                var projectBlog = new ProjectBlog()
                {
                    Id = Guid.NewGuid().ToString(),
                    BlogId = blog.Id,
                    ProjectId = request.Id,
                    Index = request.Blogs.IndexOf(blog)
                };
                await _unitOfWork.GetRepository<ProjectBlog>().AddAsync(projectBlog);
            }
            await _unitOfWork.Commit();
            return await Result.SuccessAsync("Updated Project Blogs successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> DeleteProject(string id, string userId)
    {
        try
        {
            var project = await _unitOfWork.GetRepository<Project>().Entities
                .Include(x => x.UserProjects)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
                throw new Exception("Project not found!");
            if (project.UserProjects.All(x => x.UserId != userId))
                throw new Exception("You cannot delete a project that you didn't contribute");

            foreach (var userProject in project.UserProjects)
            {
                await _unitOfWork.GetRepository<UserProject>().DeleteAsync(userProject);
            }

            await _unitOfWork.GetRepository<Project>().DeleteAsync(project);
            await _unitOfWork.Commit(); 
            return await Result.SuccessAsync("Deleted Successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}