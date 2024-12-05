namespace LibraryDemo.Core.Validation;

public class CreateBookRentalCommandValidator : AbstractValidator<CreateBookRentalCommand>
{
    public CreateBookRentalCommandValidator()
    {
        RuleFor(b => b.UserId)
            .NotEmpty().WithMessage("{UserId} is required.");

        RuleFor(b => b.BookId)
            .NotEmpty().WithMessage("{BookId} is required.");

        RuleFor(b => b.DueDate)
            .NotEmpty().WithMessage("{DueDate} is required.");
    }
}