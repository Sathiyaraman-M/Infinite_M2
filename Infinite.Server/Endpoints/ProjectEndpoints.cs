using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;

namespace Infinite.Server.Endpoints;

public static class ProjectEndpoints
{
    public static void RegisterProjectEndpoints(this WebApplication app)
    {
        var project = app.MapGroup("api/project").RequireAuthorization().WithOpenApi().WithTags("Projects");

        project.MapGet("authors/get",async ([FromBody] string searchString, [FromServices] IProjectService projectService) =>
            Results.Ok(await projectService.SearchAuthors(searchString)));

        project.MapGet("blogs/search", async ([FromBody] GetBlogsRequest request, [FromServices] IProjectService projectService) => 
            Results.Ok(await projectService.SearchBlogs(request)));

        project.MapGet("{id}", async (string id, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetProject(id, userId));
        });
        
        project.MapGet("{id}/authors", async (string id, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetProjectAuthors(id, userId));
        });
        
        project.MapGet("{id}/blogs", async (string id, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetProjectBlogs(id, userId));
        });
        
        project.MapGet("recent", async (string exclude, int? n, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetRecentNProjects(userId, userId, exclude, n ?? 4));
        });
        
        project.MapGet("recent/{authorId}", async (string authorId, string exclude, int? n, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetRecentNProjects(authorId, userId, exclude, n ?? 4));
        });
        
        project.MapGet("", async (int pageNumber, int pageSize, string searchString, string authorId, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.GetAllProjects(pageNumber, pageSize, searchString, authorId, userId));
        });
        
        project.MapPost("publish", async (EditProjectRequest request, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.SaveProject(request, userId));
        });
        
        project.MapPost("blogs", async (ProjectBlogsRequest request, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.SaveProjectBlogs(request, userId));
        });
        
        project.MapDelete("blogs", async (string id, HttpContext context, IProjectService projectService) =>
        {
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            return Results.Ok(await projectService.DeleteProject(id, userId));
        });
    }
}