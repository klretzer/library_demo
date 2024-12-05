namespace LibraryDemo.Core.Validation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("{Email} is required.")
            .MaximumLength(256).WithMessage("{Email} must not exceed 256 characters.")
            .EmailAddress().WithMessage("{Email} must be a valid e-mail address.");

        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage("{UserName} is required.")
            .MaximumLength(256).WithMessage("{UserName} must not exceed 256 characters.");

        RuleFor(r => r.Role)
            .NotEmpty().WithMessage("{Role} is required.")
            .MaximumLength(256).WithMessage("{Role} must not exceed 256 characters.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("{Password} is required.");
    }
}