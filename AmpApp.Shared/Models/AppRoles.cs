namespace AmpApp.Shared.Models;

public static partial class AppRoles
{
    public const string Guest = "Guest";
    public const string Editor = "Editor";
    public const string Supervisor = "Supervisor";
    public const string Admin = "Admin";
    public const string HelpAuthor = "HelpAuthor";

    // these are used for authorization in the client; the api uses AppPolicies of the same name
    public static string EditorOrHigher => $"{Editor},{Supervisor}";
    public static string GuestOrHigher => $"{Guest},{Editor},{Supervisor}";
}