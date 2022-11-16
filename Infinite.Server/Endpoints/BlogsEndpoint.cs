using System.Security.Claims;
using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Endpoints;

public static class BlogsEndpoint
{
    public static void RegisterBlogEndpoints(this WebApplication app)
    {
        var blogs = app.MapGroup("api/blogs").RequireAuthorization().WithOpenApi().WithTags("Blogs & BlogDrafts");
        
        blogs.MapGet("{id}", async (string id, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetFullBlog(id, userId));
        }).WithName("Get a complete blog");
        
        blogs.MapGet("draft/{id}", async (string id, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetFullBlogDraft(id, userId));
        }).WithName("Get a complete blog draft");
        
        blogs.MapGet("recent", async (string exclude, int? n, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetRecentNBlogs(userId, userId, exclude, n ?? 4));
        }).WithName("Get my recent blogs");
        
        blogs.MapGet("recent/{authorId}", async (string authorId, string exclude, int? n, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetRecentNBlogs(authorId, userId, exclude, n ?? 4));
        }).WithName("Get recent blogs of specified author");
        
        blogs.MapGet("recent/drafts", async (int? n, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetRecentNDrafts(userId, n ?? 4));
        }).WithName("Get my recent blog drafts");

        blogs.MapGet("", async (int pageNumber, int pageSize, string searchString, string authorId, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetAllBlogs(pageNumber, pageSize, searchString, authorId, userId));
        }).WithName("Get all my blogs with pagination");
        
        blogs.MapGet("drafts", async (int pageNumber, int pageSize, string searchString, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.GetAllDrafts(pageNumber, pageSize, searchString, userId));
        }).WithName("Get all my blog drafts with pagination");
        
        blogs.MapPost("publish", async (EditBlogRequest editBlogRequest, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.PublishAsync(editBlogRequest, userId));
        }).WithName("Publish new blog or update existing blog");
        
        blogs.MapPost("draft", async (EditBlogRequest editBlogRequest, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.SaveToDraftsAsync(editBlogRequest, userId));
        }).WithName("Save a blog draft or update existing blog draft");
        
        blogs.MapDelete("{id}", async (string id, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.DeleteBlogAsync(id, userId));
        }).WithName("Delete a blog");
        
        blogs.MapDelete("draft/{id}", async (string id, HttpContext context, IBlogService blogService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await blogService.DiscardDraftAsync(id, userId));
        }).WithName("Discard a blog");
    }
}