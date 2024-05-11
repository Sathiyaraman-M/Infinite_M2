using Duende.IdentityServer.Extensions;
using Infinite.Base.Entities;
using Infinite.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Server.Authorization;

public class FileAuthorizationHandler(IUnitOfWork unitOfWork) : AuthorizationHandler<FileAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FileAuthorizationRequirement requirement)
    {
        try
        {
            if (!context.User.IsAuthenticated())
            {
                throw new Exception("Not authenticated");
            }
            var userId = context.User.FindFirstValue(JwtClaimTypes.Subject);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("userId not Found");
            }
            var path = requirement.FolderDirectory.Trim();
            if (path == HostingExtensions.NotFileAccess)
            {
                context.Succeed(requirement);
                return;
            }
            var userIdFound = await unitOfWork.GetRepository<AppUser>().Entities.AnyAsync(x => x.Id == path);
            if (userIdFound)
            {
                context.Succeed(requirement);
            }
            else
            {
                var projectIdFound = await unitOfWork.GetRepository<UserProject>().Entities
                    .AnyAsync(x => x.UserId == userId && x.ProjectId == path);
                if (projectIdFound)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    throw new Exception("File Access Not authenticated");
                }
            }
        }
        catch (Exception e)
        {
            context.Fail();
            Console.WriteLine(e);
        }
    }
}