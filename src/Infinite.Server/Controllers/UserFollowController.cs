using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/follow")]
[ApiController]
public class UserFollowController(IUserFollowService userFollowService) : ControllerBase
{
    [HttpGet("followed")]
    public async Task<IActionResult> GetFollowedPeopleDetails(int pageNumber = 1, int pageSize = 10, string searchString = "", string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.GetFollowedPeopleDetails(pageNumber, pageSize, searchString, string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followed/all")]
    public async Task<IActionResult> GetAllFollowedPeopleDetails(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.GetAllFollowedPeopleDetails(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowersDetails(int pageNumber = 1, int pageSize = 10, string searchString = "", string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.GetFollowersDetails(pageNumber, pageSize, searchString, string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followers/all")]
    public async Task<IActionResult> GetAllFollowersDetails(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.GetAllFollowersDetails(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> IsUserFollowed(string authorId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.IsUserFollowed(userId, authorId);
        return Ok(result);
    }

    [HttpGet("toggle")]
    public async Task<IActionResult> ToggleUserFollow(string followedId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.ToggleUserFollow(userId, followedId);
        return Ok(result);
    }

    [HttpGet("stat")]
    public async Task<IActionResult> GetFollowStat(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await userFollowService.GetFollowStatResponse(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }
}