using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Zamp.Models;

namespace Zamp.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Returns the part of the email address before the '@' symbol.
    /// Returns null if input is null, empty, or does not contain '@'.
    /// </summary>
    public static string? GetUsernameFromEmail(this string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var atIndex = email.IndexOf('@');
        return atIndex <= 0 ? null : email[..atIndex];
    }

    public static string PrependQuestionMark(this string queryString)
        => string.IsNullOrEmpty(queryString) ? "" : "?" + queryString;

    public static bool ContainsHtmlTags(this string? checkString)
    {
        return Regex.IsMatch(checkString ?? "", "<[a-zA-Z]{1,2}>"); // not exhaustive but good enough for our purposes
    }

    public static string? Append(this string? textExisting, string? textToAppend, string separator = "")
    {
        if (string.IsNullOrWhiteSpace(textExisting))
            return textToAppend;
        return textExisting + separator + textToAppend;
    }

    public static string? HtmlToText(this string? html)
    {
        if (html == null)
            return null;

        var s = html;
        s = s.Replace("<p>", "");
        s = s.Replace("</p>\r\n", "\n");
        s = s.Replace("</p>\n", "\n");
        s = s.Replace("</p>", "\n");
        s = s.Replace("<b>", "");
        s = s.Replace("</b>", "");
        s = s.Replace("<i>", "");
        s = s.Replace("</i>", "");
        s = s.Replace("<u>", "");
        s = s.Replace("</u>", "");
        s = s.Replace("<ul>", "\n");
        s = s.Replace("</ul>", "");
        s = s.Replace("<ol>", "\n");
        s = s.Replace("</ol>", "");
        s = s.Replace("<li>", "  - ");
        s = s.Replace("</li>", "\n");
        s = s.Replace("<br/>", "\n");
        s = s.Replace("&nbsp;", " ");
        return s;
    }

    public static T? FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public static string? AllowOnlyPermittedFileNameCharacters(this string? source)
    {
        return source == null
            ? null
            : Regex.Replace(source.Replace("’", "'"), "[^A-Za-z0-9_.,()'\x20\x2D]", "");
    }

    public static string? Cleanse(this string? source)
    {
        return source == null
            ? null
            : Regex.Replace(source, "[^A-Za-z0-9_.]", "");
    }

    public static string ReplaceTextFromTo(
        this string? source,
        string startingText,
        string endingText,
        string replacementText,
        bool throwExceptionIfNotFound = true)
    {
        if (source is null) return string.Empty;
        
        string regexPattern = $"""{startingText}[\s\S]*{endingText}""";
        if (throwExceptionIfNotFound && !Regex.IsMatch(source, regexPattern))
            throw new InvalidDataException($"Text not found: {startingText}...{endingText}");
        return Regex.Replace(source, regexPattern, replacementText);
    }

    public static string ExtractNameFromEmail(this string email)
    {
        return email.Split('@')[0].Replace("'", "");
    }

    public static string FormatSql(this string sql, bool setUserNameSessionContext = true)
    {
        var s = sql;
        s = s.Replace("\r", "\n");
        while (s.Contains("\n\n"))
        {
            s = s.Replace("\n\n", "\n");
        }

        s = s.Trim();
        //TODO: Remove Replace("_","") -- used as a quick and dirty to turn Postgres SQL into SQL Server SQL
        // s = s.Replace("SCOPE_IDENTITY()", "SCOPE~IDENTITY()");
        // s = s.Trim().Replace("_","");
        // s = s.Replace("SCOPE~IDENTITY()", "SCOPE_IDENTITY()");

        if (setUserNameSessionContext)
        {
            s = "EXEC sys.sp_set_session_context @key = N'CurrentUserLoginUserName', @value = @CurrentUserLoginUserName;\n" + s;
        }

        return s;
    }

    public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
    {
        if (string.IsNullOrEmpty(value) || maxLength <= 0) return value;
        return value.Length <= maxLength ? value : value[..maxLength] + (addEllipsis ? "\u2026" : "");
    }

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static List<string> DeserializeJsonList(this string? jsonData)
    {
        return jsonData.DeserializeJsonList<KeyValueModel>().Select(id => id.Value ?? string.Empty).ToList();
    }

    public static List<TReturn> DeserializeJsonList<TReturn>(this string? jsonData)
    {
        if (typeof(TReturn) == typeof(string))
            throw new InvalidDataException("To return a list of string, the JSON must be KeyValueModel (i.e. SELECT x AS Value WHERE ...) and you must use DeserializeJsonList (without specifying a return type).");

        if (string.IsNullOrEmpty(jsonData))
            return [];

        return JsonSerializer.Deserialize<List<TReturn>>(jsonData, _jsonSerializerOptions) ?? [];
    }

    public static string? StripCharacters(this string source, string charactersToStrip)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return string.Join("", source.Split(charactersToStrip.ToCharArray()));
    }

    public static string? StripCharactersForSearch(this string source)
    {
        return StripCharacters(source, "- '")?.ToLower();
    }

    public static string ToSnakeCase(this string source)
    {
        // converts camelCase or PascalCase to snake_case
        var snakeCase = "";
        foreach (char c in char.ToLower(source[0]) + source[1..])
        {
            var s = c.ToString();
            if (s != s.ToLower()) // only true for A-Z
                snakeCase += "_";
            snakeCase += s.ToLower();
        }

        return snakeCase;
    }

    public static string ToPascalCase(this string str)
    {
        // convert snake_case and kebab-case to PascalCase
        var words = str.Replace("-", "_").Split('_');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = words[i].FirstCharToUpper();
            }
        }

        return string.Join("", words);
    }

    public static string FromPascalCaseToHeading(this string str, string[]? wordsToCapitalize = null)
    {
        switch (str.ToLower())
        {
            case "canadiancharterofrights":
                return "Canadian Charter Of Rights";
        }

        var s = Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
        foreach (var word in wordsToCapitalize ?? [])
        {
            s = s.Replace(word, word.ToUpper());
        }

        return s;
    }

    public static string? ConcatWith(this string? str0, string? str1, string separator = ": ")
    {
        if (string.IsNullOrEmpty(str0))
            return str1;
        return string.IsNullOrEmpty(str1) ? str0 : $"{str0}{separator}{str1}";
    }

    public static string? ToTitleCase(this string? text)
    {
        return text is null ? null : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
    }

    private static string FirstCharToUpper(this string? input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

    public static string? SeparateAtUpper(this string? source, bool pluralize, string separator = " ")
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        var returnValue = "";
        var isFirstCharacter = true;
        foreach (char c in source)
        {
            var s = c.ToString();
            if (isFirstCharacter)
                isFirstCharacter = false;
            else if (s != s.ToLower()) // only true for A-Z
                returnValue += separator;
            returnValue += s;
        }

        if (pluralize)
        {
            returnValue = returnValue.Plural();
        }

        return returnValue;
    }

    public static string? Plural(this string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        if (source[^1..] != "y" || source[^2..] == "ay" || source[^2..] == "ey" || source[^2..] == "oy")
            return $"{source}s";
        else
            return $"{source[0..^1]}ies";
    }

    // public static string? ToTitleCase(this string? source)
    // {
    // 	if (string.IsNullOrWhiteSpace(source))
    // 		return null;
    //
    // 	return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLower());
    // }

    public static string? Extension(this string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return Path.GetExtension(source).ToLower().Replace(".", "");
    }

    public static bool IsIn<T>(this T source, params T[] values)
    {
        return values.Contains(source);
    }

    public static string? Substring(this string? sourceString, string? from = null, string? until = null, StringComparison comparison = StringComparison.InvariantCulture)
    {
        if (string.IsNullOrWhiteSpace(sourceString))
            return null;

        var fromLength = (from ?? string.Empty).Length;
        var startIndex = !string.IsNullOrEmpty(from)
            ? sourceString.IndexOf(from, comparison) + fromLength
            : 0;

        if (startIndex < fromLength)
            return null; // Failed to find an instance of the first anchor

        var endIndex = !string.IsNullOrEmpty(until)
            ? sourceString.IndexOf(until, startIndex, comparison)
            : sourceString.Length;

        if (endIndex < 0)
            return null; // Failed to find an instance of the last anchor

        var subString = sourceString[startIndex..endIndex];
        return subString;
    }
}