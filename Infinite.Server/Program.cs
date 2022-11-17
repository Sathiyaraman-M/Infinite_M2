global using System.Security.Claims;
global using IdentityModel;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
using Infinite.Server;

WebApplication
    .CreateBuilder()
    .ConfigureServices()
    .ConfigureMiddlewares()
    //.RegisterEndpoints()
    .Run();