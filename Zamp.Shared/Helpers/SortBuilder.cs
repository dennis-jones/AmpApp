using Zamp.Shared.Extensions;

namespace Zamp.Shared.Helpers;

using System.Text;
using System.Collections.Generic;

public class SortEntry
{
    public required string Column { get; set; }
    public bool Ascending { get; set; }
}
public class SortBuilder
{
    public List<SortEntry> Sorts { get; set; } = [];

    public int MaxSortKeys { get; set; }

    public SortBuilder(int maxSortKeys = 3)
    {
        MaxSortKeys = maxSortKeys > 0 ? maxSortKeys : 3;
    }

    public bool IsSortingSetUp => Sorts.Count > 0;

    private static bool IsValidColumnName(string? column)
    {
        if (string.IsNullOrWhiteSpace(column))
            return false;
        return column.All(x => char.IsLetterOrDigit(x) || x == '_' || x == '.');
    }

    public SortBuilder AddSort(string? columnNames, bool ascending = true)
    {
        if (string.IsNullOrWhiteSpace(columnNames))
            return this;

        // Remove whitespace and validate
        columnNames = columnNames.Replace(" ", "");
        if (!columnNames.All(x => char.IsLetterOrDigit(x) || x == ',' || x == '_' || x == '.'))
            return this;

        var columns = columnNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Where(IsValidColumnName)
                                 .ToArray();

        if (columns.Length == 0)
            return this;

        // Check if these columns are already the primary sort columns, in order
        bool isSame =
            columns.Length <= Sorts.Count &&
            columns.SequenceEqual(Sorts.Take(columns.Length).Select(s => s.Column), StringComparer.OrdinalIgnoreCase);

        if (isSame)
        {
            // Toggle sort order for all matching columns
            bool newOrder = !Sorts[0].Ascending;
            for (int i = 0; i < columns.Length; i++)
                Sorts[i] = new SortEntry { Column = Sorts[i].Column, Ascending = newOrder };
            return this;
        }

        // Otherwise, insert new columns as primary, in order, and remove duplicates
        foreach (var col in columns.Reverse())
        {
            Sorts.RemoveAll(s => string.Equals(s.Column, col, StringComparison.OrdinalIgnoreCase));
            Sorts.Insert(0, new SortEntry { Column = col, Ascending = ascending });
        }
        TrimSorts();
        return this;
    }

    public SortBuilder Clear()
    {
        Sorts.Clear();
        return this;
    }

    public SortBuilder SetSort(params (string? column, bool ascending)[] sorts)
    {
        Sorts.Clear();
        foreach (var sort in sorts)
        {
            if (IsValidColumnName(sort.column))
                Sorts.Add(new SortEntry { Column = sort.column!, Ascending = sort.ascending });
        }
        TrimSorts();
        return this;
    }

    public string OrderByClause()
    {
        if (Sorts.Count == 0)
            return string.Empty;

        var sb = new StringBuilder("ORDER BY ");
        for (var i = 0; i < Sorts.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(Sorts[i].Column.DatabaseColumnName());
            if (!Sorts[i].Ascending) sb.Append(" DESC");
        }
        return sb.ToString();
    }

    private void TrimSorts()
    {
        if (Sorts.Count > MaxSortKeys)
            Sorts.RemoveRange(MaxSortKeys, Sorts.Count - MaxSortKeys);
    }
}