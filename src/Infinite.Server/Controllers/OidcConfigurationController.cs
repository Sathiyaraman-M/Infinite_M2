using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;

namespace Infinite.Server.Controllers;

public class OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider) : Controller
{
    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        var parameters = clientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        return Ok(parameters);
    }
}