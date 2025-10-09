namespace Zamp.Shared.Models;

public class GridCriteriaModel
{
    public bool DisablePagination { get; set; }
    public int Offset { get; set; }
    public int PageSize { get; set; } = 3;
    public SortBuilder GridSorting { get; set; } = new();

}

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