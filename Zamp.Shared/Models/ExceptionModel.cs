namespace Zamp.Shared.Models;

public class ExceptionModel
{
    public required string Code { get; set; }
    public int? StatusCode { get; set; }
    public required string Description { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Hint { get; set; }
    public string? RawMessage { get; set; }
    public string? StackTrace { get; set; }
    public List<ValidationError> ValidationErrors { get; set; } = [];
}

public class ValidationError(string propertyName, string errorMessage)
{
    public string PropertyName { get; set; } = propertyName;
    public string ErrorMessage { get; set; } = errorMessage;
}
