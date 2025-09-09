using Zamp.Shared.Helpers;

namespace Zamp.Shared.Models.Criteria;

public class GridCriteriaModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
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