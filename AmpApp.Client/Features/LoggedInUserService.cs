
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Zamp.Client.Features.LoggedInUser;

namespace AmpApp.Client.Features;

public class LoggedInUserService(AuthenticationStateProvider authProvider, IJSRuntime jsRuntime) 
    : LoggedInUserServiceBase(authProvider, jsRuntime)
{
    // public TodoSearchCriteria SimpleSearchCriteria { get; set; }
    // public TodoSearchCriteria AdvancedSearchCriteria { get; set; }

    public bool IsCaseSearchSimple { get; set; }

}