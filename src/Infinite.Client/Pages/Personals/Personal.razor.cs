using System.Net.Http.Json;
using System.Security.Claims;
using BlazorSlice.Dialog;
using BlazorSlice.Dialog.Services;
using Infinite.Base.Responses;
using Infinite.Client.Shared.Dialogs;
using Markdig;
using Size = BlazorSlice.Dialog.Size;

namespace Infinite.Client.Pages.Personals;

public partial class Personal
{
    private string _userPortFolioMd;
    private List<MinifiedProjectResponse> _myLast4Projects = new();
    private List<AuthorPublicInfoResponse> _myFollowedDetails = new();
    private List<MinifiedBlogResponse> _myLast4Blogs = new();
    private List<MinifiedBlogDraftResponse> _myLast4BlogDrafts = new();
    private bool _isMdLoaded;
    private bool _isProjectsLoaded;
    private bool _isFollowedLoaded;
    private bool _isBlogsLoaded;
    private bool _isBlogDraftsLoaded;

    private static MarkdownPipeline Pipeline => new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseEmojiAndSmiley()
        .Build();

    protected override async Task OnInitializedAsync()
    {
        await LoadPortfolio();
        await LoadProjects();
        await LoadFollowedDetails();
        await LoadLatestBlogs();
        await LoadLatestBlogDrafts();
        await base.OnInitializedAsync();
        _isMdLoaded = true;
        _isProjectsLoaded = true;
        _isFollowedLoaded = true;
        _isBlogsLoaded = true;
        _isBlogDraftsLoaded = true;
    }

    private async Task LoadPortfolio()
    {
        var portfolioResult = await HttpClient.GetFromJsonAsync<Result<string>>("api/manage/portfolio");
        if (portfolioResult!.Succeeded)
        {
            _userPortFolioMd = portfolioResult.Data;
        }
        else
        {
            foreach (var message in portfolioResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task LoadFollowedDetails()
    {
        var result = await UserFollowHttpClient.GetAllFollowedDetails();
        if (result.Succeeded)
        {
            _myFollowedDetails = result.Data;
        }
        else
        {
            foreach (var message in result.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task LoadProjects()
    {
        var projectsResult = await ProjectHttpClient.GetRecentNProjects();
        if (projectsResult!.Succeeded)
        {
            _myLast4Projects = projectsResult.Data;
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
        var blogsResult = await BlogHttpClient.GetRecentNBlogs();
        if (blogsResult!.Succeeded)
        {
            _myLast4Blogs = blogsResult.Data;
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
        var blogDraftResult = await BlogHttpClient.GetRecentNBlogDrafts();
        if (blogDraftResult!.Succeeded)
        {
            _myLast4BlogDrafts = blogDraftResult.Data;
        }
        else
        {
            foreach (var message in blogDraftResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task DeleteProjectAsync(string id)
    {
        var options = new DialogOptions() { Position = DialogPosition.Centered };
        if (await DialogService.ShowMessageBox("Confirm Delete",
                "Are you sure want to delete this project? This action cannot be undone", "Yes", "No", options:options) == true)
        {
            var result = await ProjectHttpClient.DeleteProject(id);
            if (result!.Succeeded)
            {
                Toast.Add("Success", result.Messages[0], Severity.Success);
                await LoadProjects();
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    Toast.Add("Error", message, Severity.Error);
                }
            }
        }
    }

    private async Task DeleteBlogAsync(string id)
    {
        var options = new DialogOptions() { Position = DialogPosition.Centered };
        if (await DialogService.ShowMessageBox("Confirm Delete",
                "Are you sure want to delete this blog? This action cannot be undone", "Yes", "No", options:options) == true)
        {
            var result = await BlogHttpClient.DeleteBlog(id);
            if (result!.Succeeded)
            {
                Toast.Add("Success", result.Messages[0], Severity.Success);
                await LoadLatestBlogs();
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    Toast.Add("Error", message, Severity.Error);
                }
            }
        }
    }

    private async Task DiscardDraftAsync(string id)
    {
        var options = new DialogOptions() { Position = DialogPosition.Centered };
        if (await DialogService.ShowMessageBox("Confirm Discard",
                "Are you sure want to discard this blog draft? This action cannot be undone", "Yes", "No", options: options) == true)
        {
            var result = await BlogHttpClient.DiscardBlogDraft(id);
            if (result!.Succeeded)
            {
                Toast.Add("Success", result.Messages[0], Severity.Success);
                await LoadLatestBlogDrafts();
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    Toast.Add("Error", message, Severity.Success);
                }
            }
        }
    }

    private async Task InvokeShareDialog()
    {
        var userId = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.FindFirstValue("sub");
        var parameters = new DialogParameters()
        {
            { nameof(ShareLinkDialog.Link), NavigationManager.ToAbsoluteUri($"/{userId}").ToString() }
        };
        var options = new DialogOptions()
        {
            Position = DialogPosition.Centered,
            Size = Size.Small
        };
        DialogService.Show<ShareLinkDialog>("Share your Profile", parameters, options);
    }
}