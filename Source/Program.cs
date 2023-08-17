global using FastEndpoints;
global using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestIdentity.Data;

var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(bld.Configuration.GetConnectionString("DefaultConnection")))
   .AddFastEndpoints()
   .AddJWTBearerAuth("Token_Signing_Key")
   .AddDefaultIdentity<IdentityUser>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager();

//setting jwt auth scheme to be the default.
//jwt will be tried first to authenticate all incoming requests,
//unless otherwise specified at the endpoint level.
bld.Services
   .AddAuthentication(o => o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme);

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints();
app.Run();