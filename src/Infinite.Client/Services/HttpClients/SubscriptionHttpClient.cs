using System.Net.Http.Json;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Infinite.Client.Extensions;

namespace Infinite.Client.Services.HttpClients;

public class SubscriptionHttpClient(HttpClient httpClient)
{
    public async Task<IResult<UserSubscriptionResponse>> GetUserSubscriptionDetails()
    {
        return await httpClient.GetFromJsonAsync<Result<UserSubscriptionResponse>>("api/subscription");
    }

    public async Task<IResult> UpdateUserSubscriptionDetails(UpdateSubscriptionRequest request)
    {
        var response =  await httpClient.PostAsJsonAsync("api/subscription", request);
        return await response.ToResult();
    }
}