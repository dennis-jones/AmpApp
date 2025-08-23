namespace Zamp.Extensions;

public static class DateOnlyExtensions
{
    public static string? FormatDate(this DateOnly? value)
        => value?.ToString("MMM d, yyyy").Replace(".", "");
}