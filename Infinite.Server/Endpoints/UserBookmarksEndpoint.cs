using System.Security.Claims;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Endpoints;

public static class UserBookmarksEndpoint
{
    public static void RegisterUserBookmarksEndpoints(this WebApplication app)
    {
        var bookmark = app.MapGroup("api/user/bookmarks").RequireAuthorization().WithOpenApi().WithTags("User Bookmarks");
        
        bookmark.MapGet("blog/get", async (int pageNumber, int pageSize, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.GetBookmarkedBlogs(pageNumber, pageSize, userId));
        });
        
        bookmark.MapGet("project/get", async (int pageNumber, int pageSize, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.GetBookmarkedProjects(pageNumber, pageSize, userId));
        });
        
        bookmark.MapGet("blog/{id}", async (string id, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.IsBlogBookmarked(id, userId));
        });
        
        bookmark.MapGet("project/{id}", async (string id, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.IsProjectBookmarked(id, userId));
        });
        
        bookmark.MapGet("blog/{id}/toggle", async (string id, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.ToggleBlogBookmark(id, userId));
        });
        
        bookmark.MapGet("project/{id}/toggle", async (string id, HttpContext context, IUserBookmarkService userBookmarkService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userBookmarkService.ToggleProjectBookmark(id, userId));
        });
    }
}