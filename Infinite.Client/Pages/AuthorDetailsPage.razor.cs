using System.Net.Http.Json;
using System.Security.Claims;
using BlazorSlice.Dialog;
using BlazorSlice.Dialog.Services;
using Infinite.Client.Shared.Dialogs;
using Infinite.Base.Responses;
using Markdig;
using Size = BlazorSlice.Dialog.Size;

namespace Infinite.Client.Pages;

public partial class AuthorDetailsPage
{
    [Parameter]
    public Guid Id { get; set; }
    
    private string _userPortFolioMd;
    private AuthorPublicInfoResponse _authorPublicInfo;
    private List<MinifiedBlogResponse> _latest4Blogs = new();
    private bool _isMdLoaded;
    private bool _isBlogsLoaded;

    private static MarkdownPipeline Pipeline => new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseEmojiAndSmiley()
        .Build();

    protected override async Task OnParametersSetAsync()
    {
        await LoadPortfolio();
        await LoadAuthorPublicDetails();
        await LoadLatestBlogs();
        await base.OnParametersSetAsync();
        _isMdLoaded = true;
        _isBlogsLoaded = true;
    }

    private async Task LoadPortfolio()
    {
        var portfolioResult = await ManageAccountHttpClient.GetUserPortfolio(Id.ToString());
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

    private async Task LoadAuthorPublicDetails()
    {
        var portfolioResult = await ManageAccountHttpClient.GetAuthorPublicDetails(Id.ToString());
        if (portfolioResult!.Succeeded)
        {
            _authorPublicInfo = portfolioResult.Data;
        }
        else
        {
            foreach (var message in portfolioResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task LoadLatestBlogs()
    {
        var blogsResult = await BlogHttpClient.GetRecentNBlogs(Id.ToString());
        if (blogsResult!.Succeeded)
        {
            _latest4Blogs = blogsResult.Data;
        }
        else
        {
            foreach (var message in blogsResult.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
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
        DialogService.Show<ShareLinkDialog>("Share this person", parameters, options);
    }
}