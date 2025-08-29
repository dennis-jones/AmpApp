using System.Globalization;
using System.Text.Json;
using AmpApp.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Zamp.Server.Infrastructure.Middleware;
using Zamp.Shared.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

var returnRawErrors = builder.Configuration.GetValue<bool>("ErrorHandling:ReturnRawErrors");

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

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();

builder.Services.AddCarter();

// Registers all FluentValidation validators from the Shared assembly.
builder.Services.AddValidatorsFromAssemblyContaining<AmpApp.Shared.AssemblyMarker>();
ValidatorOptions.Global.LanguageManager = new FluentValidationLanguageManager();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en"); // this line must be after setting the LanguageManager

builder.Services.AddHttpContextAccessor();
// builder.Services.AddScoped<UserContext>();
builder.Services.AddInjectables(typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAntiforgery();

app.UseMiddleware<ApiExceptionHandlingMiddleware>(returnRawErrors);
app.UseMiddleware<ApiAuthorizationErrorMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AmpApp.Client._Imports).Assembly);

app.MapCarter();

app.Run();