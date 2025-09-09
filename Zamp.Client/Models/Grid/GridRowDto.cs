namespace Zamp.Client.Models;

public abstract class GridRowDto
{
    public Guid Id { get; set; }
    public bool IsSelected { get; set; }
    public string? CssClass { get; set; }
}