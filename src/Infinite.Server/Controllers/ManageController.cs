using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;
using Microsoft.AspNetCore.Authentication;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/manage")]
[ApiController]
public class ManageController : ControllerBase
{
    private readonly IManageAccountService _manageAccountService;

    public ManageController(IManageAccountService manageAccountService)
    {
        _manageAccountService = manageAccountService;
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetAuthorPublicDetails(string id)
    {
        var result = await _manageAccountService.GetAuthorPublicDetails(id);
        return Ok(result);
    }
    
    [HttpGet("portfolio")]
    public async Task<IActionResult> GetUserPortFolio(string authorId = null)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _manageAccountService.GetPortFolioMd(string.IsNullOrEmpty(authorId) ? userId : authorId);
        return Ok(result);
    }

    [HttpPost("portfolio")]
    public async Task<IActionResult> SaveCurrentUserPortFolio(MarkdownModel model)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _manageAccountService.SavePortFolio(userId, model.Markdown);
        return Ok(result);
    }
    
    [HttpGet("profileInfo")]
    public async Task<IActionResult> GetCurrentUserProfileInfo()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        var result = await _manageAccountService.GetUserProfileInfo(userId);
        return Ok(result);
    }

    [HttpPost("profileInfo")]
    public async Task<IActionResult> SaveCurrentUserProfileInfo(UpdateUserProfileInfoRequest request)
    {
        var result = await _manageAccountService.UpdateUserProfileInfo(request);
        return Ok(result);
    }

    [HttpDelete("account")]
    public async Task<IActionResult> DeleteInfiniteAccount()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        await HttpContext.SignOutAsync();
        var result = await _manageAccountService.DeleteInfiniteAccount(userId);
        return Ok(result);
    }

    public class MarkdownModel
    {
        public string Markdown { get; set; }
    }
}