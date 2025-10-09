namespace AmpApp.Shared.Models.Todo;

public class TodoEntity : EntityBase
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
}

public class TodoEntityValidator : AbstractValidator<TodoEntity>
{
    public TodoEntityValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Description)
            .MaximumLength(100);
    }
}