namespace TestIdentity.JwtEndpoints;

sealed class JwtLoginEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("jwt-login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var jwt = JWTBearer.CreateToken("Token_Signing_Key", expireAt: DateTime.UtcNow.AddYears(10));
        await SendAsync(jwt);
    }
}