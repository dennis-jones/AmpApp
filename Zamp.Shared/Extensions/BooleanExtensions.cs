namespace Zamp.Extensions;

public static class BooleanExtensions
{
    public static string? ToYesNo(this bool? value, string? nullValue = null) 
        => value switch
        {
            true => "Yes",
            false => "No",
            _ => nullValue
        };

    public static string ToYesNo(this bool value) 
        => value ? "Yes" : "No";
}