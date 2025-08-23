using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Zamp.Extensions;

namespace Zamp.Client.Features.LoggedInUser;

public class LoggedInUserServiceBase(AuthenticationStateProvider authProvider) : IScopedInjectable
{
    public LoggedInUserModel User { get; } = new();
    
    public async Task InitializeAsync()
    {
        var authState = await authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        User.IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
        User.Name = user.Identity?.Name.GetUsernameFromEmail() ?? string.Empty;
        User.Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        User.Roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        
        User.IsAdmin = User.Roles.Contains("Admin");
        
        User.IsSupervisor = User.Roles.Contains("Supervisor");
        User.IsEditorOrHigher = User.Roles.Contains("Editor"); // server project will assign Editor role if you are Editor or Supervisor (see program.cs)
        User.IsGuestOrHigher = User.Roles.Contains("Guest"); // server project will assign Guest role if you are Guest or Editor or Supervisor (see program.cs)
        
        User.IsHelpAuthor = User.Roles.Contains("HelpAuthor");
    }
}