using FluentValidation;

namespace Cloud_Mall.Application.DeliveryOffer.Command.CreateDeliveryOffer
{
    public class CreateDeliveryOfferCommandValidator : AbstractValidator<CreateDeliveryOfferCommand>
    {
        public CreateDeliveryOfferCommandValidator()
        {
            RuleFor(x => x.CustomerOrderID)
                .GreaterThan(0).WithMessage("Customer order ID must be greater than 0");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.EstimatedDays)
                .GreaterThan(0).WithMessage("Estimated days must be greater than 0")
                .LessThanOrEqualTo(30).WithMessage("Estimated days cannot exceed 30 days");
        }
    }
} 