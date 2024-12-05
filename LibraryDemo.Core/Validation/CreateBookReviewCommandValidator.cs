namespace LibraryDemo.Core.Validation;

public class CreateBookReviewCommandValidator : AbstractValidator<CreateBookReviewCommand>
{
    public CreateBookReviewCommandValidator()
    {
        RuleFor(b => b.UserId)
            .NotEmpty().WithMessage("{UserId} is required.");

        RuleFor(b => b.BookId)
            .NotEmpty().WithMessage("{BookId} is required.");

        RuleFor(b => b.Rating)
            .NotEmpty().WithMessage("{Rating} is required.")
            .InclusiveBetween(1, 5).WithMessage("{Rating} must be an integer between 1 and 5.");

        RuleFor(b => b.Text)
            .MaximumLength(512).WithMessage("{Text} must not exceed 512 characters.");
    }
}