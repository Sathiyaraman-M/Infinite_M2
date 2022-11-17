using System.Net.Http.Json;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Infinite.Client.Extensions;

namespace Infinite.Client.Services.HttpClients;

public class BlogHttpClient
{
    private readonly HttpClient _httpClient;

    public BlogHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<FullBlogResponse>> GetFullBlog(string id)
    {
        return await _httpClient.GetFromJsonAsync<Result<FullBlogResponse>>($"api/blogs/{id}");
    }

    public async Task<IResult<FullBlogDraftResponse>> GetFullBlogDraft(string id)
    {
        return await _httpClient.GetFromJsonAsync<Result<FullBlogDraftResponse>>($"api/blogs/draft/{id}");
    }

    public async Task<IResult<List<MinifiedBlogResponse>>> GetRecentNBlogs(string authorId = null, string exclude = null,
        int n = 4)
    {
        return string.IsNullOrEmpty(authorId)
            ? await _httpClient.GetFromJsonAsync<Result<List<MinifiedBlogResponse>>>(
                $"api/blogs/recent?exclude={exclude}&n={n}")
            : await _httpClient.GetFromJsonAsync<Result<List<MinifiedBlogResponse>>>(
                $"api/blogs/recent/{authorId}?exclude={exclude}&n={n}");
    }

    public async Task<IResult<List<MinifiedBlogDraftResponse>>> GetRecentNBlogDrafts(int n= 4)
    {
        return await _httpClient.GetFromJsonAsync<Result<List<MinifiedBlogDraftResponse>>>(
            $"api/blogs/recent/drafts?n={n}");
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetAllBlogs(int pageNumber, int pageSize,
        string searchString, string authorId)
    {
        return await _httpClient.GetFromJsonAsync<PaginatedResult<MinifiedBlogResponse>>(
            $"api/blogs?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}");
    }

    public async Task<PaginatedResult<MinifiedBlogDraftResponse>> GetAllBlogDrafts(int pageNumber, int pageSize,
        string searchString)
    {
        return await _httpClient.GetFromJsonAsync<PaginatedResult<MinifiedBlogDraftResponse>>(
            $"api/blogs/drafts?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}");
    }

    public async Task<IResult> PublishAsync(EditBlogRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/blogs/publish", request);
        return await response.ToResult();
    }

    public async Task<IResult> SaveToDrafts(EditBlogRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/blogs/draft", request);
        return await response.ToResult();
    }

    public async Task<IResult> DeleteBlog(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/blogs/{id}");
        return await response.ToResult();
    }

    public async Task<IResult> DiscardBlogDraft(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/blogs/draft/{id}");
        return await response.ToResult();
    }
}