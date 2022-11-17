using System.Net.Http.Json;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Infinite.Client.Extensions;

namespace Infinite.Client.Services.HttpClients;

public class ManageAccountHttpClient
{
    private readonly HttpClient _httpClient;

    public ManageAccountHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<AuthorPublicInfoResponse>> GetAuthorPublicDetails(string id)
    {
        return await _httpClient.GetFromJsonAsync<Result<AuthorPublicInfoResponse>>($"api/manage/details?id={id}");
    }

    public async Task<IResult<string>> GetUserPortfolio(string id = null)
    {
        return await _httpClient.GetFromJsonAsync<Result<string>>($"api/manage/portfolio?id={id}");
    }

    public async Task<IResult> SaveUserPortfolio(string markdown)
    {
        var response = await _httpClient.PostAsJsonAsync("api/manage/portfolio", new { Markdown = markdown });
        return await response.ToResult();
    }

    public async Task<IResult<UserProfileInfoResponse>> GetUserProfileInfo()
    {
        return await _httpClient.GetFromJsonAsync<Result<UserProfileInfoResponse>>("api/manage/profileInfo");
    }

    public async Task<IResult> SaveUserProfileInfo(UpdateUserProfileInfoRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/manage/profileInfo", request);
        return await response.ToResult();
    }

    public async Task<IResult> DeleteAccountPermanently()
    {
        var response = await _httpClient.DeleteAsync("api/manage/account");
        return await response.ToResult();
    }
}