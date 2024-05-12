global using FastEndpoints;
global using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestIdentity.Data;

var bld = WebApplication.CreateBuilder(args);

bld.Services
   .AddFastEndpoints()
   .AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(bld.Configuration.GetConnectionString("DefaultConnection")));

//register jwt auth scheme
bld.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = "A_Secret_Token_Signing_Key_Longer_Than_32_Characters");

//register identity/cookie auth scheme
bld.Services.AddDefaultIdentity<IdentityUser>()
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddSignInManager();

//override the behavior or cookie auth scheme so that 401/403 will be returned.
bld.Services
   .ConfigureApplicationCookie(
       c =>
       {
           c.Events.OnRedirectToLogin
               = ctx =>
                 {
                     if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                         ctx.Response.StatusCode = 401;

                     return Task.CompletedTask;
                 };
           c.Events.OnRedirectToAccessDenied
               = ctx =>
                 {
                     if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                         ctx.Response.StatusCode = 403;

                     return Task.CompletedTask;
                 };
       });

//setting jwt auth scheme to be the default. jwt will be tried first to authenticate all incoming requests, unless otherwise specified at the endpoint level.
bld.Services.AddAuthentication(o => o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme);
bld.Services.AddAuthorization();

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

await app.Services
         .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>()
         .Database
         .EnsureCreatedAsync();

app.Run();