using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/likes")]
public class UserLikesController : ControllerBase
{
    private readonly IUserLikesService _userLikesService;

    public UserLikesController(IUserLikesService userLikesService)
    {
        _userLikesService = userLikesService;
    }

    [HttpGet("blog/{id}")]
    public async Task<IActionResult> GetBlogLikeStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.IsBlogLiked(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/{id}/toggle")]
    public async Task<IActionResult> ToggleBlogLike(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.ToggleBlogLike(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/get")]
    public async Task<IActionResult> GetLikedBlogs(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.GetLikedBlogs(pageNumber, pageSize, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProjectLikeStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.IsProjectLiked(id, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}/toggle")]
    public async Task<IActionResult> ToggleProjectLike(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.ToggleProjectLike(id, userId);
        return Ok(result);
    }

    [HttpGet("project/get")]
    public async Task<IActionResult> GetLikedProjects(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userLikesService.GetLikedProjects(pageNumber, pageSize, userId);
        return Ok(result);
    }
}