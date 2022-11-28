using Microsoft.Extensions.Options;

namespace Infinite.Server.Authorization;

public class FileAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    
    public FileAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        var isGuid = Guid.TryParse(policyName, out var guid);
        if (!isGuid)
        {
            return !string.IsNullOrWhiteSpace(policyName)
                ? FallbackPolicyProvider.GetPolicyAsync(HostingExtensions.NotFileAccess)
                : FallbackPolicyProvider.GetFallbackPolicyAsync();
        }
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new FileAuthorizationRequirement(policyName));
        return Task.FromResult(policy.Build());
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => Task.FromResult<AuthorizationPolicy>(null);
}