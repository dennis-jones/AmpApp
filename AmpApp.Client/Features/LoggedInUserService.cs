
using Microsoft.AspNetCore.Components.Authorization;
using Zamp.Client.Features.LoggedInUser;

namespace AmpApp.Client.Features;

public class LoggedInUserService(AuthenticationStateProvider authProvider) : LoggedInUserServiceBase(authProvider)
{
    // public TodoSearchCriteria SimpleSearchCriteria { get; set; }
    // public TodoSearchCriteria AdvancedSearchCriteria { get; set; }

    public bool IsCaseSearchSimple { get; set; }

}