using Zamp.Shared.Models.Criteria;

namespace AmpApp.Shared.Models.Todo;

public class TodoSearchCriteria : PaginationModel
{
    public bool? IsSimpleSearch { get; set; } = true;
    
    // Simple Search criteria
    public string? Text { get; set; }
    
     // Advanced Search criteria
   public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; } = false;
}