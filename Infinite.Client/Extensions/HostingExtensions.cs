using BlazorSlice.Dialog.Extensions;
using BlazorSlice.Toast.Extensions;
using Infinite.Client.Services;
using Infinite.Client.Services.HttpClients;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Infinite.Client.Extensions;

public static class HostingExtensions
{
    public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        return builder;
    }

    public static WebAssemblyHostBuilder ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.ConfigureHttpClients(builder.HostEnvironment.BaseAddress);
        // builder.Services.AddOidcAuthentication(options =>
        // {
        //     builder.Configuration.Bind("Local", options.ProviderOptions);
        // });
        builder.Services.AddApiAuthorization();
        builder.Services.AddBlazorSliceToast();
        builder.Services.AddBlazorSliceDialog();
        builder.Services.AddSingleton<NavigationService>();
        return builder;
    }

    private static void ConfigureHttpClients(this IServiceCollection services, string baseAddress)
    {
        services.AddHttpClient("Infinite.ServerAPI", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Infinite.ServerAPI"));
        services.AddHttpClient<PublicHttpClient>(client => client.BaseAddress = new Uri(baseAddress));
        services.AddTransient<BlogHttpClient>();
        services.AddTransient<ProjectHttpClient>();
        services.AddTransient<ManageAccountHttpClient>();
        services.AddTransient<UserBookmarkHttpClient>();
        services.AddTransient<UserLikeHttpClient>();
        services.AddTransient<UserFollowHttpClient>();
        services.AddTransient<SubscriptionHttpClient>();
    }

    public static WebAssemblyHost InvokeStartupServices(this WebAssemblyHost host)
    {
        host.Services.GetRequiredService<NavigationService>();
        return host;
    }
}