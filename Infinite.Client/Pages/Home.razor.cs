using System.Net.Http.Json;
using Infinite.Base.Responses;

namespace Infinite.Client.Pages;

public partial class Home
{
    private List<MinifiedProjectResponse> _myLast12Projects = new();
    private List<MinifiedBlogResponse> _myLast12Blogs = new();
    private List<MinifiedBlogDraftResponse> _myLast12BlogDrafts = new();
    private bool _isProjectsLoaded;
    private bool _isBlogsLoaded;
    private bool _isBlogDraftsLoaded;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadProjects();
        await LoadLatestBlogs();
        await LoadLatestBlogDrafts();
        await base.OnInitializedAsync();
        _isProjectsLoaded = true;
        _isBlogsLoaded = true;
        _isBlogDraftsLoaded = true;
    }
    
    private async Task LoadProjects()
    {
        var projectsResult = await HttpClient.GetFromJsonAsync<Result<List<MinifiedProjectResponse>>>("api/projects/recent?n=12");
        if (projectsResult!.Succeeded)
        {
            _myLast12Projects = projectsResult.Data;
        }
        else
        {
            foreach (var message in projectsResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task LoadLatestBlogs()
    {
        var blogsResult = await HttpClient.GetFromJsonAsync<Result<List<MinifiedBlogResponse>>>("api/blogs/recent?n=12");
        if (blogsResult!.Succeeded)
        {
            _myLast12Blogs = blogsResult.Data;
        }
        else
        {
            foreach (var message in blogsResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task LoadLatestBlogDrafts()
    {
        var blogDraftResult = await HttpClient.GetFromJsonAsync<Result<List<MinifiedBlogDraftResponse>>>("api/blogs/recent/drafts?n=12");
        if (blogDraftResult!.Succeeded)
        {
            _myLast12BlogDrafts = blogDraftResult.Data;
        }
        else
        {
            foreach (var message in blogDraftResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }
}