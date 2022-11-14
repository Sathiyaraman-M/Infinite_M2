using System.Security.Claims;
using Infinite.Base.Requests;
using Infinite.Core.Interfaces.Features;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Endpoints;

public static class ManageAccountEndpoints
{
    public static void RegisterManageAccountEndpoints(this WebApplication app)
    {
        var manageAccount = app.MapGroup("api/manage").RequireAuthorization().WithOpenApi().WithTags("Manage Account");
        
        manageAccount.MapGet("details", async (string id, IManageAccountService manageAccountService) => 
            Results.Ok(await manageAccountService.GetAuthorPublicDetails(id)));

        manageAccount.MapGet("portfolio",async (string authorId, HttpContext context, IManageAccountService manageAccountService) =>
        {
            var userId = context.User.FindFirstValue("sub");
            return Results.Ok(await manageAccountService.GetPortFolioMd(string.IsNullOrEmpty(authorId) ? userId : authorId));
        });
        
        manageAccount.MapPost("portfolio", async (MarkdownModel model, HttpContext context, IManageAccountService manageAccountService) =>
        {
            var userId = context.User.FindFirstValue("sub");
            return Results.Ok(await manageAccountService.SavePortFolio(userId, model.Markdown));
        });

        manageAccount.MapGet("profileInfo",async (HttpContext context, IManageAccountService manageAccountService) =>
        {
            var userId = context.User.FindFirstValue("sub");
            return Results.Ok(await manageAccountService.GetUserProfileInfo(userId));
        });

        manageAccount.MapPost("profileInfo",async (UpdateUserProfileInfoRequest request, IManageAccountService manageAccountService) => 
            Results.Ok(await manageAccountService.UpdateUserProfileInfo(request)));

        manageAccount.MapDelete("account", async (HttpContext context, IManageAccountService manageAccountService) =>
        {
            var userId = context.User.FindFirstValue("sub");
            await context.SignOutAsync();
            return Results.Ok(await manageAccountService.DeleteInfiniteAccount(userId));
        });
    }

    private class MarkdownModel
    {
        public string Markdown { get; set; }
    }
}