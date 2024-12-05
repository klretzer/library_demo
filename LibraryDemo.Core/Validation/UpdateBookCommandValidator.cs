namespace LibraryDemo.Core.Validation;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("{Title} is required.")
            .MaximumLength(64).WithMessage("{Title} must not exceed 64 characters.");

        RuleFor(b => b.Author)
            .NotEmpty().WithMessage("{Author} is required.")
            .MaximumLength(64).WithMessage("{Author} must not exceed 64 characters.");

        RuleFor(b => b.Description)
            .NotEmpty().WithMessage("{Description} is required.");

        RuleFor(b => b.Category)
            .NotEmpty().WithMessage("{Category} is required.")
            .MaximumLength(64).WithMessage("{Category} must not exceed 64 characters.");

        RuleFor(b => b.Publisher)
            .NotEmpty().WithMessage("{Publisher} is required.")
            .MaximumLength(64).WithMessage("{Publisher} must not exceed 64 characters.");

        RuleFor(b => b.CoverImageUrl)
            .MaximumLength(256).WithMessage("{CoverImageUrl} must not exceed 256 characters.");

        RuleFor(b => b.PageCount)
            .NotEmpty().WithMessage("{PageCount} is required.")
            .GreaterThan(0).WithMessage("{PageCount} must be > 0.");

        RuleFor(b => b.ISBN)
            .NotEmpty().WithMessage("{ISBN} is required.");

        RuleFor(b => b.PublicationDate)
            .NotEmpty().WithMessage("{PublicationDate} is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("{PublicationDate} must be prior to today's date.");
    }
}