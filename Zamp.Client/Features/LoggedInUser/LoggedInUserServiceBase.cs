using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace Zamp.Client.Features.LoggedInUser;

public class LoggedInUserServiceBase(AuthenticationStateProvider authenticationStateProvider, IJSRuntime jsRuntime) : IScopedInjectable
{
    public LoggedInUserModel User { get; } = new();
    public int LocalTimeZoneOffset { get; set; }
    public string LocalTimeZoneName { get; set; } = "";
    public TimeZoneInfo LocalTimeZoneInfo { get; set; } = default!;
    
    public async Task InitializeAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        User.IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
        User.Name = user.Identity?.Name.GetUsernameFromEmail() ?? string.Empty;
        User.Email = user.Identity?.Name ?? string.Empty;

        User.Claims = user.Claims;
        User.Roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        
        User.IsAdmin = User.Roles.Contains("Admin");
        
        User.IsSupervisor = User.Roles.Contains("Supervisor");
        User.IsEditorOrHigher = User.Roles.Contains("Supervisor") || User.Roles.Contains("Editor"); // server project will assign Editor role if you are Editor or Supervisor (see program.cs)
        User.IsGuestOrHigher = User.Roles.Contains("Supervisor") || User.Roles.Contains("Editor") || User.Roles.Contains("Guest"); // server project will assign Guest role if you are Guest or Editor or Supervisor (see program.cs)
        
        User.IsHelpAuthor = User.Roles.Contains("HelpAuthor");
        
         
        LocalTimeZoneOffset = 0 - await jsRuntime.InvokeAsync<int>("getLocalTimeOffset", []);
        LocalTimeZoneName = await jsRuntime.InvokeAsync<string>("getLocalTimeZoneName", []);
        LocalTimeZoneInfo = TimeZoneConverter.TZConvert.GetTimeZoneInfo(LocalTimeZoneName);
        // 2024-01-10: TimeZoneInfo.FindSystemTimeZoneById(timeZoneName) does not work on Azure; need to use TimeZoneConverter nuget package
           
    }
}