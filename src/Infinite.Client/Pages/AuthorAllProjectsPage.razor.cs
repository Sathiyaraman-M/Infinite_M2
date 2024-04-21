using Infinite.Base.Responses;

namespace Infinite.Client.Pages;

public partial class AuthorAllProjectsPage
{
    [Parameter]
    public Guid Id { get; set; }
    
    private int _pageNumber;
    private int _pageSize;
    private int _totalPages;
    private string _searchString;
    private AuthorPublicInfoResponse _authorPublicInfo;
    private List<MinifiedProjectResponse> _projects = new();

    private async Task Search(string value)
    {
        _searchString = value;
        _pageNumber = 1;
        await LoadData(_pageNumber);
    }

    protected override async Task OnInitializedAsync()
    {
        _pageNumber = 1;
        _pageSize = 12;
        await base.OnInitializedAsync();
        await LoadData(1);
        await LoadAuthorPublicDetails();
    }

    private async Task LoadData(int pageNumber)
    {
        var result = await ProjectHttpClient.GetAllProjects(pageNumber, _pageSize,_searchString, Id.ToString());
        if (result!.Succeeded)
        {
            _projects = result.Data;
            _pageNumber = result.CurrentPage;
            _pageSize = result.PageSize;
            _totalPages = result.TotalPages;
        }
        else
        {
            foreach (var message in result.Messages)
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

    private async Task ToggleBookmark(string projectId)
    {
        var result = await UserBookmarkHttpClient.ToggleProjectBookmark(Guid.Parse(projectId));
        if (result.Succeeded)
        {
            await LoadData(_pageNumber);
            Toast.Add("Success", result.Data ? "Added to bookmarks successfully" : "Removed from bookmarks successfully", Severity.Info);
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