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
        var jwt = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = "A_Secret_Token_Signing_Key_Longer_Than_32_Characters";
                o.ExpireAt = DateTime.UtcNow.AddYears(10);
            });

        await SendAsync(jwt);
    }
}