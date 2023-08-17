using Microsoft.AspNetCore.Identity;

namespace TestIdentity.IdentityEndpoints;

sealed class IdentityLoginEndpoint : EndpointWithoutRequest
{
    public SignInManager<IdentityUser> SignInManager { get; set; } = default!;

    public override void Configure()
    {
        Get("idn-login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var user = new IdentityUser
        {
            UserName = "test-user-001",
            Email = "test@domain.com"
        };

        await SignInManager.SignInAsync(user, true);

        await SendAsync("You are signed in!");
    }
}