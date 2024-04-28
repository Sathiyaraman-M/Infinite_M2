using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace Infinite.Core.Persistence;

public class AppDbContext : ApiAuthorizationDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions, IOptions<OperationalStoreOptions> operationalStoreOptions) 
        : base(dbContextOptions, operationalStoreOptions)
    {
        
    }
    
    public DbSet<UserProfileInfo> UserProfileInfos { get; set; }
    public DbSet<UserPortfolio> UserPortfolios { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<UserCurrentSubscription> SubscriptionHistory { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogDraft> BlogDrafts { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectBlog> ProjectBlogs { get; set; }
    public DbSet<UserProject> UserProjects { get; set; }
    public DbSet<BlogBookmark> UserBlogBookmarks { get; set; }
    public DbSet<ProjectBookmark> UserProjectBookmarks { get; set; }
    public DbSet<BlogLike> UserBlogLikes { get; set; }
    public DbSet<ProjectLike> UserProjectLikes { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
        base.OnModelCreating(builder);
    }
}