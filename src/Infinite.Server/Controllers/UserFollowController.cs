using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/user/follow")]
[ApiController]
public class UserFollowController : ControllerBase
{
    private readonly IUserFollowService _userFollowService;

    public UserFollowController(IUserFollowService userFollowService)
    {
        _userFollowService = userFollowService;
    }

    [HttpGet("followed")]
    public async Task<IActionResult> GetFollowedPeopleDetails(int pageNumber = 1, int pageSize = 10, string searchString = "", string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.GetFollowedPeopleDetails(pageNumber, pageSize, searchString, string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followed/all")]
    public async Task<IActionResult> GetAllFollowedPeopleDetails(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.GetAllFollowedPeopleDetails(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followers")]
    public async Task<IActionResult> GetFollowersDetails(int pageNumber = 1, int pageSize = 10, string searchString = "", string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.GetFollowersDetails(pageNumber, pageSize, searchString, string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet("followers/all")]
    public async Task<IActionResult> GetAllFollowersDetails(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.GetAllFollowersDetails(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> IsUserFollowed(string authorId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.IsUserFollowed(userId, authorId);
        return Ok(result);
    }

    [HttpGet("toggle")]
    public async Task<IActionResult> ToggleUserFollow(string followedId)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.ToggleUserFollow(userId, followedId);
        return Ok(result);
    }

    [HttpGet("stat")]
    public async Task<IActionResult> GetFollowStat(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _userFollowService.GetFollowStatResponse(string.IsNullOrWhiteSpace(authorId) ? userId : authorId);
        return Ok(result);
    }
}