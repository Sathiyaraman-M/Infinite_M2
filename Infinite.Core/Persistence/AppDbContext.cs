using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infinite.Core.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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
}