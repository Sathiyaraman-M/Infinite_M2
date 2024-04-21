namespace Infinite.Server.Middlewares;

public class FileServerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly PathString _pathString;

    public FileServerMiddleware(RequestDelegate next, PathString pathString)
    {
        _next = next;
        var isGuid = Guid.TryParse(_pathString, out var result);
        if (isGuid)
        {
            _pathString = pathString;
        }
    }

    public async Task Invoke(HttpContext context, IAuthorizationService authorizationService)
    {
        if (context.Request.Path.StartsWithSegments(_pathString, StringComparison.InvariantCultureIgnoreCase, out var matched, out var remaining))
        {
            var authorized = await authorizationService.AuthorizeAsync(context.User, matched);
        }
        await _next(context);
    }
}