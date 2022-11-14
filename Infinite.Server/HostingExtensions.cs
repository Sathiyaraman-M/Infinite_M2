using Infinite.Base.Entities;
using Infinite.Core.Features;
using Infinite.Core.Interfaces.Features;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Core.Persistence;
using Infinite.Core.Repositories;
using Infinite.Server.Endpoints;
using Infinite.Server.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Server;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            });
        builder.Services.AddAuthorization();
        builder.Services.ConfigureInternalServices();
        builder.Services.ConfigureFeatures();
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

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
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

    private static void ConfigureInternalServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
    }

    private static void ConfigureFeatures(this IServiceCollection services)
    {
        services.AddTransient<IManageAccountService, ManageAccountService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IUserBookmarkService, UserBookmarkService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IUserLikesService, UserLikesService>();
    }

    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.RegisterBlogEndpoints();
        app.RegisterManageAccountEndpoints();
        app.RegisterProjectEndpoints();
        app.RegisterUserLikesEndpoints();
        app.RegisterUserBookmarksEndpoints();
        return app;
    }
}