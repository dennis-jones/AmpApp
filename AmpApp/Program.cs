using AmpApp.Components;
using AmpApp.Shared.Models;
using Carter;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// builder.Services.ConfigureApiCookieEvents();

// Authorization for App Roles
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy; // by default, authorization of endpoints is required
    options.AddPolicy(AppPolicies.Supervisor, policy =>
        policy.RequireRole(AppRoles.Supervisor));
    options.AddPolicy(AppPolicies.EditorOrHigher, policy =>
        policy.RequireRole(AppRoles.Editor, AppRoles.Supervisor));
    options.AddPolicy(AppPolicies.GuestOrHigher, policy =>
        policy.RequireRole(AppRoles.Guest, AppRoles.Editor, AppRoles.Supervisor));
    options.AddPolicy(AppPolicies.Admin, policy => policy.RequireRole(AppRoles.Admin));
    options.AddPolicy(AppPolicies.HelpAuthor, policy => policy.RequireRole(AppRoles.HelpAuthor));
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AmpApp.Client._Imports).Assembly);

app.MapCarter();
app.Run();