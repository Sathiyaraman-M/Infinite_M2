using System.Net.Http.Json;
using Infinite.Base.Responses;

namespace Infinite.Client.Services.HttpClients;

public class UserLikeHttpClient(HttpClient httpClient)
{
    public static UserLikeHttpClient UseCustomHttpClient(HttpClient httpClient)
    {
        return new UserLikeHttpClient(httpClient);
    }
    
    public async Task<IResult<LikeResponse>> IsBlogLiked(Guid blogId)
    {
        return await httpClient.GetFromJsonAsync<Result<LikeResponse>>($"api/user/likes/blog/{blogId}");
    }
    
    public async Task<IResult<LikeResponse>> ToggleBlogLike(Guid blogId)
    {
        return await httpClient.GetFromJsonAsync<Result<LikeResponse>>($"api/user/likes/blog/{blogId}/toggle");
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetLikedBlogs(int pageNumber, int pageSize)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<MinifiedBlogResponse>>(
            $"api/user/likes/blog/get?pageNumber={pageNumber}&pageSize={pageSize}");
    }
    
    public async Task<IResult<LikeResponse>> IsProjectLiked(Guid projectId)
    {
        return await httpClient.GetFromJsonAsync<Result<LikeResponse>>($"api/user/likes/project/{projectId}");
    }
    
    public async Task<IResult<LikeResponse>> ToggleProjectLike(Guid projectId)
    {
        return await httpClient.GetFromJsonAsync<Result<LikeResponse>>($"api/user/likes/project/{projectId}/toggle");
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetLikedProjects(int pageNumber, int pageSize)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<MinifiedProjectResponse>>(
            $"api/user/likes/project/get?pageNumber={pageNumber}&pageSize={pageSize}");
    }
}