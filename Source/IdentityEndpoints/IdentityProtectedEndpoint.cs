using Microsoft.AspNetCore.Identity;

namespace TestIdentity.IdentityEndpoints;

sealed class IdentityProtectedEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("idn-protected");

        //only identity/cookie auth scheme is allowed
        AuthSchemes(IdentityConstants.ApplicationScheme);
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        await SendAsync("You are authenticated via MS Identity!");
    }
}