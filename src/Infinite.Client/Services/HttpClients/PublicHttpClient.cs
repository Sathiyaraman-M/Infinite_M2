namespace Infinite.Client.Services.HttpClients;

public class PublicHttpClient(HttpClient httpClient)
{
    public HttpClient HttpClient { get; } = httpClient;
}