global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Infinite.Base.Wrapper;
using Infinite.Client.Extensions;

WebAssemblyHostBuilder.CreateDefault(args)
    .AddRootComponents()
    .ConfigureServices()
    .Build()
    .InvokeStartupServices()
    .RunAsync();