using FluentValidation;

namespace Cloud_Mall.Application.DeliveryCompany.Command.RegisterDeliveryCompany
{
    public class RegisterDeliveryCompanyCommandValidator : AbstractValidator<RegisterDeliveryCompanyCommand>
    {
        public RegisterDeliveryCompanyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters");

            RuleFor(x => x.CommercialSerialNumber)
                .NotEmpty().WithMessage("Commercial serial number is required")
                .MaximumLength(50).WithMessage("Commercial serial number cannot exceed 50 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");
        }
    }
} 