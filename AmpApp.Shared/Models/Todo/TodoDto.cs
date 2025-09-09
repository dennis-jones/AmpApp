namespace AmpApp.Shared.Models.Todo;

public class TodoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
}