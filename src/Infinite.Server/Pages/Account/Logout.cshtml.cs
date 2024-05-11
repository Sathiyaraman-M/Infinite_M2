using Infinite.Base.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Infinite.Server.Pages.Account;

public class LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger) : PageModel
{
    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return RedirectToPage();
    }
}