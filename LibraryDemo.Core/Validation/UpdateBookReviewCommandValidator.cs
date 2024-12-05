namespace LibraryDemo.Core.Validation;

public class UpdateBookReviewCommandValidator : AbstractValidator<UpdateBookReviewCommand>
{
    public UpdateBookReviewCommandValidator()
    {
        RuleFor(b => b.Rating)
            .NotEmpty().WithMessage("{Rating} is required.")
            .InclusiveBetween(1, 5).WithMessage("{Rating} must be an integer between 1 and 5.");

        RuleFor(b => b.Text)
            .MaximumLength(512).WithMessage("{Text} must not exceed 512 characters.");
    }
}