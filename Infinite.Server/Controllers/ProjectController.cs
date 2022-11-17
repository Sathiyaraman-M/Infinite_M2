using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("authors/get")]
    public async Task<IActionResult> SearchAuthors(string searchString)
    {
        var result = await _projectService.SearchAuthors(searchString);
        return Ok(result);
    }

    [HttpPost("blogs/search")]
    public async Task<IActionResult> SearchBlogs(GetBlogsRequest request)
    {
        var result = await _projectService.SearchBlogs(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.GetProject(id, userId);
        return Ok(result);
    }
    
    [HttpGet("{id}/blogs")]
    public async Task<IActionResult> GetProjectBlogs(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.GetProjectBlogs(id, userId);
        return Ok(result);
    }

    [HttpGet("{id}/authors")]
    public async Task<IActionResult> GetProjectAuthors(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.GetProjectAuthors(id, userId);
        return Ok(result);
    }

    [HttpGet("recent")]
    [HttpGet("recent/{authorId}")]
    public async Task<IActionResult> GetRecentNProjects(string authorId = null, string exclude = null, int n = 4)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.GetRecentNProjects(string.IsNullOrEmpty(authorId) ? userId : authorId, userId, exclude, n);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects(int pageNumber, int pageSize, string searchString, string authorId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.GetAllProjects(pageNumber, pageSize, searchString, authorId, userId);
        return Ok(result);
    }


    [HttpPost("publish")]   
    public async Task<IActionResult> SaveProject(EditProjectRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.SaveProject(request, userId);
        return Ok(result);
    }

    [HttpPost("blogs")]
    public async Task<IActionResult> SaveProjectBlogs(ProjectBlogsRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.SaveProjectBlogs(request, userId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _projectService.DeleteProject(id, userId);
        return Ok(result);
    }
}