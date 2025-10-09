namespace AmpApp.Shared.Models.Todo;

public class TodoGridCriteriaModel : GridCriteriaModel
{
    public bool? IsSimpleSearch { get; set; } = true;
    
    // Simple Search criteria
    public string? Text { get; set; }
    
    // Advanced Search criteria
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsComplete { get; set; } = false;
    
}