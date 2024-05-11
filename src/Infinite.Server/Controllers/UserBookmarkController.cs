using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/bookmarks")]
[ApiController]
public class UserBookmarkController(IUserBookmarkService userBookmarkService) : ControllerBase
{
    [HttpGet("blog/{id}")]
    public async Task<IActionResult> GetBlogBookmarkStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.IsBlogBookmarked(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/{id}/toggle")]
    public async Task<IActionResult> ToggleBlogBookmark(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.ToggleBlogBookmark(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/get")]
    public async Task<IActionResult> GetBookmarkBlogs(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.GetBookmarkedBlogs(pageNumber, pageSize, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProjectBookmarkStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.IsProjectBookmarked(id, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}/toggle")]
    public async Task<IActionResult> ToggleProjectBookmark(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.ToggleProjectBookmark(id, userId);
        return Ok(result);
    }

    [HttpGet("project/get")]
    public async Task<IActionResult> GetBookmarkProjects(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userBookmarkService.GetBookmarkedProjects(pageNumber, pageSize, userId);
        return Ok(result);
    }
}