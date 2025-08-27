namespace AmpApp.Shared.Models.Todo;

public class TodoSearchResult
{
    public List<TodoDto> Rows { get; set; } = [];
    public int TotalCount { get; set; }
}