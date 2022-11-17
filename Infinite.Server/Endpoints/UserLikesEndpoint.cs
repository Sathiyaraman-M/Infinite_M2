using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Endpoints;

public static class UserLikesEndpoint
{
    public static void RegisterUserLikesEndpoints(this WebApplication app)
    {
        var like = app.MapGroup("api/user/likes").RequireAuthorization().WithOpenApi().WithTags("User Likes");

        like.MapGet("blog/get", async (int pageNumber, int pageSize, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.GetLikedBlogs(pageNumber, pageSize, userId));
        });
        
        like.MapGet("project/get", async (int pageNumber, int pageSize, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.GetLikedProjects(pageNumber, pageSize, userId));
        });
        
        like.MapGet("blog/{id}", async (string id, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.IsBlogLiked(id, userId));
        });
        
        like.MapGet("project/{id}", async (string id, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.IsProjectLiked(id, userId));
        });
        
        like.MapGet("blog/{id}/toggle", async (string id, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.ToggleBlogLike(id, userId));
        });
        
        like.MapGet("project/{id}/toggle", async (string id, HttpContext context, IUserLikesService userLikesService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await userLikesService.ToggleProjectLike(id, userId));
        });
    }
}