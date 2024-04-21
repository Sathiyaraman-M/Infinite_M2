using System.Net.Http.Json;
using Infinite.Base.Responses;

namespace Infinite.Client.Services.HttpClients;

public class UserBookmarkHttpClient
{
    private readonly HttpClient _httpClient;

    public UserBookmarkHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public static UserBookmarkHttpClient UseCustomHttpClient(HttpClient httpClient)
    {
        return new UserBookmarkHttpClient(httpClient);
    }

    public async Task<IResult<bool>> IsBlogBookmarked(Guid blogId)
    {
        return await _httpClient.GetFromJsonAsync<Result<bool>>($"api/user/bookmarks/blog/{blogId}");
    }
    
    public async Task<IResult<bool>> ToggleBlogBookmark(Guid blogId)
    {
        return await _httpClient.GetFromJsonAsync<Result<bool>>($"api/user/bookmarks/blog/{blogId}/toggle");
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetBookmarkBlogs(int pageNumber, int pageSize)
    {
        return await _httpClient.GetFromJsonAsync<PaginatedResult<MinifiedBlogResponse>>(
            $"api/user/bookmarks/blog/get?pageNumber={pageNumber}&pageSize={pageSize}");
    }

    public async Task<IResult<bool>> IsProjectBookmarked(Guid blogId)
    {
        return await _httpClient.GetFromJsonAsync<Result<bool>>($"api/user/bookmarks/project/{blogId}");
    }
    
    public async Task<IResult<bool>> ToggleProjectBookmark(Guid blogId)
    {
        return await _httpClient.GetFromJsonAsync<Result<bool>>($"api/user/bookmarks/project/{blogId}/toggle");
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetBookmarkProjects(int pageNumber, int pageSize)
    {
        return await _httpClient.GetFromJsonAsync<PaginatedResult<MinifiedProjectResponse>>(
            $"api/user/bookmarks/project/get?pageNumber={pageNumber}&pageSize={pageSize}");
    }
}