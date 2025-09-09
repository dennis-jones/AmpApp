using System.Text;

namespace Zamp.Shared.Extensions;

public static class StringPropertyNameToColumnNameExtensions
{
    public static string DatabaseColumnName(this string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return string.Empty;

        string s = propertyName switch
        {
            "Id" => "id",
            // Add any other exceptions here (NOTE: Id is not an exception but included to show the pattern)
            _ => ToSnakeCase(propertyName)
        };
        return $"\"{s}\""; //safest to add double quotes around column names to eliminate possibility of conflict with reserved words

    }

    public static string ToSnakeCase(this string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c) && i > 0)
            {
                sb.Append('_');
            }

            sb.Append(char.ToLowerInvariant(c));
        }

        return sb.ToString();
    }
}