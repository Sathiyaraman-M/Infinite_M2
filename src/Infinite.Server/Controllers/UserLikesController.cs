using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/likes")]
public class UserLikesController(IUserLikesService userLikesService) : ControllerBase
{
    [HttpGet("blog/{id}")]
    public async Task<IActionResult> GetBlogLikeStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.IsBlogLiked(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/{id}/toggle")]
    public async Task<IActionResult> ToggleBlogLike(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.ToggleBlogLike(id, userId);
        return Ok(result);
    }

    [HttpGet("blog/get")]
    public async Task<IActionResult> GetLikedBlogs(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.GetLikedBlogs(pageNumber, pageSize, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProjectLikeStatus(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.IsProjectLiked(id, userId);
        return Ok(result);
    }

    [HttpGet("project/{id}/toggle")]
    public async Task<IActionResult> ToggleProjectLike(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.ToggleProjectLike(id, userId);
        return Ok(result);
    }

    [HttpGet("project/get")]
    public async Task<IActionResult> GetLikedProjects(int pageNumber, int pageSize)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userLikesService.GetLikedProjects(pageNumber, pageSize, userId);
        return Ok(result);
    }
}