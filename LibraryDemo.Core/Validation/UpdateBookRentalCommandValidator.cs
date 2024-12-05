namespace LibraryDemo.Core.Validation;

public class UpdateBookRentalCommandValidator : AbstractValidator<UpdateBookRentalCommand>
{
    public UpdateBookRentalCommandValidator()
    {
        RuleFor(b => b.DueDate)
            .NotEmpty().WithMessage("{DueDate} is required.");
    }
}