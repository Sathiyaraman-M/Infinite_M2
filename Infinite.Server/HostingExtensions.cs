using System.Net;
using Duende.IdentityServer.Extensions;
using Hangfire;
using Hangfire.Storage.SQLite;
using Infinite.Base.Entities;
using Infinite.Core.Features;
using Infinite.Core.Interfaces.Features;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Core.Interfaces.Services;
using Infinite.Core.Persistence;
using Infinite.Core.Repositories;
using Infinite.Core.Services;
using Infinite.Server.Authorization;
using Infinite.Server.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Infinite.Server;

public static class HostingExtensions
{
    internal const string NotFileAccess = "NotFileAccess";
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        //builder.Services.ConfigureFileAccessServices();
        builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddIdentityServer()
            .AddApiAuthorization<AppUser, AppDbContext>();
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            })
            .AddGitHub(options =>
            {
                options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
            });
        builder.Services.AddAuthorization();
        builder.Services.ConfigureInternalServices();
        builder.Services.ConfigureFeatures();
        builder.Services.AddHangfire(x => x.UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHangfireServer();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder.Build();
    }

    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseBlazorFrameworkFiles();
        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
        app.ConfigureFileAccessMiddleware();

        app.MapRazorPages();
        app.MapControllers();
        app.MapHangfireDashboard();
        app.MapFallbackToFile("index.html");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Novel Infinite");
            options.DisplayRequestDuration();
            options.RoutePrefix = "swagger";
        });
        
        return app;
    }

    private static void ConfigureFileAccessServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, FileAuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, FileAuthorizationHandler>();
    }

    private static void ConfigureInternalServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        services.AddTransient<IUploadService, UploadService>();
    }

    private static void ConfigureFeatures(this IServiceCollection services)
    {
        services.AddTransient<IManageAccountService, ManageAccountService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IUserBookmarkService, UserBookmarkService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IUserLikesService, UserLikesService>();
        services.AddTransient<IUserFollowService, UserFollowService>();
        services.AddTransient<ISubscriptionService, SubscriptionService>();
    }

    private static void ConfigureFileAccessMiddleware(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions()
        {
            RequestPath = new PathString("/Files"),
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            OnPrepareResponse = context =>
            {
                if (context.Context.User.IsAuthenticated()) 
                    return;
                context.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Context.Response.ContentLength = 0;
                context.Context.Response.Body = Stream.Null;
                context.Context.Response.SetNoCache();
            }
        });
    }
}