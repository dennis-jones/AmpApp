using System.Security.Claims;
using AmpApp.Shared.Models;
using Carter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace AmpApp.Features.AppUser;

public class AppUserEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/claims", (ClaimsPrincipal user) =>
        {
            return user.Claims.Select(c => new { c.Type, c.Value });
        });

        app.MapGet("/api/username", (ClaimsPrincipal user) => user.Identity?.Name);
        app.MapGet("/api/hello/anon", (ClaimsPrincipal user) => "Hello Anonymous!")
            .AllowAnonymous();
        app.MapGet("/api/hello/guest", (ClaimsPrincipal user) => "Hello Guest (or higher)!")
            .RequireAuthorization(AppPolicies.GuestOrHigher);;
        app.MapGet("/api/hello/helpauthor", (ClaimsPrincipal user) => "Hello Help Author!")
            .RequireAuthorization(AppPolicies.HelpAuthor);;

        app.MapGet("/api/account/login", (HttpContext ctx) =>
        {
            const string redirectUri = "/";
            var props = new AuthenticationProperties { RedirectUri = redirectUri };
            return ctx.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, props);
        }).AllowAnonymous();
        
        app.MapGet("/api/account/logout", [Authorize] async (HttpContext context) =>
        {
            // Sign out of Azure AD (OpenID Connect)
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            // Sign out of local cookies
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Dynamically get the base URL for post-logout redirect
            var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            var azureAdLogoutUrl = $"https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri={baseUrl}";

            // Redirect to Azure AD logout
            context.Response.Redirect(azureAdLogoutUrl);
        });
    }
}