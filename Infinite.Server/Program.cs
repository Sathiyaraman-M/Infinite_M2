global using IdentityModel;
using Infinite.Server;

WebApplication
    .CreateBuilder()
    .ConfigureServices()
    .ConfigureMiddlewares()
    .RegisterEndpoints()
    .Run();