using FluentValidation;

namespace AmpApp.Shared.Models.Todo;

public class CreateTodoDto()
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
    public CreateTodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}