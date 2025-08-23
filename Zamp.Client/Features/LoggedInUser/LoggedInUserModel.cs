namespace Zamp.Client.Features.LoggedInUser;

public class LoggedInUserModel
{
    public bool IsAuthenticated { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
    public bool IsAdmin { get; set; }
    public bool IsSupervisor { get; set; }
    public bool IsEditorOrHigher { get; set; }
    public bool IsGuestOrHigher { get; set; }
    public bool IsHelpAuthor { get; set; }

}