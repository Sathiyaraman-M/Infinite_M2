using Infinite.Base.Responses;

namespace Infinite.Client.Pages.Personals.Blogs;

public partial class AllBlogDrafts
{
    private int _pageNumber;
    private int _pageSize;
    private int _totalPages;
    private string _searchString;
    private List<MinifiedBlogDraftResponse> _blogs = new();

    protected override async Task OnInitializedAsync()
    {
        _pageNumber = 1;
        _pageSize = 12;
        await base.OnInitializedAsync();
        await LoadData(1);
    }

    private async Task LoadData(int pageNumber)
    {
        var result = await BlogHttpClient.GetAllBlogDrafts(_pageNumber, _pageSize, _searchString);
        if (result!.Succeeded)
        {
            _blogs = result.Data;
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

    private async Task Search(string value)
    {
        _searchString = value;
        await LoadData(_pageNumber);
    }

    private async Task DiscardDraftAsync(string id)
    {
        var result = await BlogHttpClient.DiscardBlogDraft(id);
        if (result!.Succeeded)
        {
            Toast.Add("Success", result.Messages[0], Severity.Success);
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