using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/blogs")]
[ApiController]
public class BlogController(IBlogService blogService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFullBlog(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetFullBlog(id, userId);
        return Ok(result);
    }

    [HttpGet("draft/{id}")]
    public async Task<IActionResult> GetFullBlogDraft(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetFullBlogDraft(id, userId);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("recent")]
    [HttpGet("recent/{authorId}")]
    public async Task<IActionResult> GetRecentNBlogs(string authorId = null, string exclude = null, int n = 4)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetRecentNBlogs(string.IsNullOrEmpty(authorId) ? userId : authorId, userId, exclude, n);
        return Ok(result);
    }

    [HttpGet("recent/drafts")]
    public async Task<IActionResult> GetRecentNDrafts(int n = 4)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetRecentNDrafts(userId, n);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs(int pageNumber, int pageSize, string searchString, string authorId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetAllBlogs(pageNumber, pageSize, searchString, string.IsNullOrEmpty(authorId) ? userId : authorId, userId);
        return Ok(result);
    }

    [HttpGet("drafts")]
    public async Task<IActionResult> GetAllDrafts(int pageNumber, int pageSize, string searchString)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.GetAllDrafts(pageNumber, pageSize, searchString, userId);
        return Ok(result);
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishAsync(EditBlogRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.PublishAsync(request, userId);
        return Ok(result);
    }

    [HttpPost("draft")]
    public async Task<IActionResult> SaveToDrafts(EditBlogRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.SaveToDraftsAsync(request, userId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogAsync(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.DeleteBlogAsync(id, userId);
        return Ok(result);
    }

    [HttpDelete("draft/{id}")]
    public async Task<IActionResult> DiscardDraftAsync(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await blogService.DiscardDraftAsync(id, userId);
        return Ok(result);
    }
}