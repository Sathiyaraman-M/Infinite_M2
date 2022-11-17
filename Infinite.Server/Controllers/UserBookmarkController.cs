using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/bookmarks")]
[ApiController]
public class UserBookmarkController : ControllerBase
{
    private readonly IUserBookmarkService _userBookmarkService;

    public UserBookmarkController(IUserBookmarkService userBookmarkService)
    {
        _userBookmarkService = userBookmarkService;
    }

    [HttpGet("blog/{id}")]
    public async Task<IActionResult> GetBlogBookmarkStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.IsBlogBookmarked(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/{id}/toggle")]
    public async Task<IActionResult> ToggleBlogBookmark(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.ToggleBlogBookmark(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/get")]
    public async Task<IActionResult> GetBookmarkBlogs(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.GetBookmarkedBlogs(pageNumber, pageSize, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProjectBookmarkStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.IsProjectBookmarked(id, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}/toggle")]
    public async Task<IActionResult> ToggleProjectBookmark(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.ToggleProjectBookmark(id, userId);
        return Ok(result);
    }

    [HttpGet("project/get")]
    public async Task<IActionResult> GetBookmarkProjects(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userBookmarkService.GetBookmarkedProjects(pageNumber, pageSize, userId);
        return Ok(result);
    }
}