using System.Net.Http.Json;
using Infinite.Base.Responses;

namespace Infinite.Client.Services.HttpClients;

public class UserFollowHttpClient(HttpClient httpClient)
{
    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowerDetails(int pageNumber, int pageSize,
        string searchString, string authorId = null)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<AuthorPublicInfoResponse>>(
            $"api/user/follow/followers?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&authorId={authorId}");
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetAllFollowerDetails(string authorId = null)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<AuthorPublicInfoResponse>>(
            $"api/user/follow/followers/all?authorId={authorId}");
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetFollowedDetails(int pageNumber, int pageSize,
        string searchString, string authorId = null)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<AuthorPublicInfoResponse>>(
            $"api/user/follow/followed?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&authorId={authorId}");
    }

    public async Task<PaginatedResult<AuthorPublicInfoResponse>> GetAllFollowedDetails(string authorId = null)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<AuthorPublicInfoResponse>>(
            $"api/user/follow/followed/all?authorId={authorId}");
    }

    public async Task<IResult<bool>> IsUserFollowed(string authorId)
    {
        return await httpClient.GetFromJsonAsync<Result<bool>>($"api/user/follow?authorId={authorId}");
    }

    public async Task<IResult<bool>> ToggleUserFollow(string followedId)
    {
        return await httpClient.GetFromJsonAsync<Result<bool>>($"api/user/follow/toggle?followedId={followedId}");
    }

    public async Task<IResult<FollowStatResponse>> GetFollowStat(string authorId = null)
    {
        return await httpClient.GetFromJsonAsync<Result<FollowStatResponse>>(
            $"api/user/follow/stat?authorId={authorId}");
    }
}