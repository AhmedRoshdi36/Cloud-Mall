using FluentValidation;

namespace Cloud_Mall.Application.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name cannot be empty!")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(v => v.Email).NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email is not a valid email address");
        }
    }
}
