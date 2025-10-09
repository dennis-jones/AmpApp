namespace Zamp.Shared.Models;

public class EntityBase
{
    public Guid? Id { get; set; }

    public int RowVersionCount { get; set; } = 0;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedUtcDateTime { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedUtcDateTime { get; set; }    
}