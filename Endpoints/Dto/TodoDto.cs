using FluentValidation;

public record TodoDto(string? Title, bool? IsComplete);

public class TodoDtoValidator : AbstractValidator<TodoDto>
{
    public TodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be less than 100 characters.");

        RuleFor(x => x.IsComplete)
            .NotNull().WithMessage("IsComplete must be specified.");
    }
}
