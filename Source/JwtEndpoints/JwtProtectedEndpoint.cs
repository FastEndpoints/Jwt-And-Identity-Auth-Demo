namespace TestIdentity.JwtEndpoints;

sealed class JwtProtectedEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("jwt-protected");

        //all registered auth schemes are allowed if none is specified.
        //since jwt is the default scheme, jwt will be used first to authenticate.

        //uncomment following to allow only jwt:
        //AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        await SendAsync("You are authenticated!");
    }
}