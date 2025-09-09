namespace AmpApp.Shared.Models.Todo;

public class TodoEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsComplete { get; set; } = false;
    public int RowVersionCount { get; set; } = 0;
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedTimestampTz { get; set; }
    public string UpdatedBy { get; set; } = default!;
    public DateTime UpdatedTimestampTz { get; set; }
}