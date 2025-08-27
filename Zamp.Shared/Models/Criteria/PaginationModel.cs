namespace Zamp.Shared.Models.Criteria;

public class PaginationModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    public string SortPropertyName1 { get; set; } = "Id";
    public bool SortAscending1 { get; set; } = true;
    public string? SortPropertyName2 { get; set; }
    public bool SortAscending2 { get; set; } = true;
    public string? SortPropertyName3 { get; set; }
    public bool SortAscending3 { get; set; } = true;
}

// public static class PaginationDefaults
// {
//     public const int PageNumber = 1;
//     public const int PageSize = 50;
//     public const string SortPropertyName1 = "Id";
//     public const string OrderBy = "id ASC";
// }

public static class PaginationAllowed
{
    public static readonly HashSet<string> PropertyNames = new(StringComparer.Ordinal)
    {
        "Id", 
        "Name", 
        "Description",
        "IsActive"
    };
}