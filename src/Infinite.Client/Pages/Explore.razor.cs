using Infinite.Base.Responses;

namespace Infinite.Client.Pages;

public partial class Explore
{
    private List<MinifiedProjectResponse> _recentlyAddedProjects = new();
    private List<MinifiedBlogResponse> _recentlyAddedBlogs = new();
    private UserProfileInfoResponse _userprofile;

    protected override async Task OnInitializedAsync()
    {
        var result = await ManageAccountHttpClient.GetUserProfileInfo();
        if (result.Succeeded)
        {
            _userprofile = result.Data;
        }
        else
        {
            foreach (var error in result.Messages)
            {
                Toast.Add("Error", error, Severity.Error);
            }
        }
    }
}