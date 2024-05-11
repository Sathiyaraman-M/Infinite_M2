using System.Net.Http.Json;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Infinite.Client.Extensions;

namespace Infinite.Client.Services.HttpClients;

public class ProjectHttpClient(HttpClient httpClient)
{
    public async Task<IResult<List<ProjectAuthorResponse>>> SearchAuthors(string searchString)
    {
        return await httpClient.GetFromJsonAsync<Result<List<ProjectAuthorResponse>>>("api/projects/authors/get");
    }

    public async Task<IResult<List<MinifiedBlogResponse>>> SearchBlogs(GetBlogsRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/projects/blogs/search", request);
        return await response.ToResult<List<MinifiedBlogResponse>>();
    }

    public async Task<IResult<FullProjectResponse>> GetFullProject(string id)
    {
        return await httpClient.GetFromJsonAsync<Result<FullProjectResponse>>($"api/projects/{id}");
    }

    public async Task<IResult<List<ProjectBlogResponse>>> GetProjectBlogs(string id)
    {
        return await httpClient.GetFromJsonAsync<Result<List<ProjectBlogResponse>>>($"api/projects/{id}/blogs");
    }

    public async Task<IResult<List<string>>> GetProjectAuthors(string id)
    {
        return await httpClient.GetFromJsonAsync<Result<List<string>>>($"api/projects/{id}/authors");
    }

    public async Task<IResult<List<MinifiedProjectResponse>>> GetRecentNProjects(string authorId = null, string exclude = null, int n = 4)
    {
        return string.IsNullOrEmpty(authorId)
            ? await httpClient.GetFromJsonAsync<Result<List<MinifiedProjectResponse>>>(
                $"api/projects/recent?exclude={exclude}&n={n}")
            : await httpClient.GetFromJsonAsync<Result<List<MinifiedProjectResponse>>>(
                $"api/projects/recent/{authorId}?exclude={exclude}&n={n}");
    }

    public async Task<PaginatedResult<MinifiedProjectResponse>> GetAllProjects(int pageNumber, int pageSize,
        string searchString, string authorId)
    {
        return await httpClient.GetFromJsonAsync<PaginatedResult<MinifiedProjectResponse>>(
            $"api/projects?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&authorId={authorId}");
    }

    public async Task<IResult<string>> SaveProject(EditProjectRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/projects/publish", request);
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> SaveProjectBlogs(ProjectBlogsRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/projects/blogs", request);
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> DeleteProject(string id)
    {
        var response = await httpClient.DeleteAsync($"api/projects/{id}");
        return await response.ToResult<string>();
    }
}