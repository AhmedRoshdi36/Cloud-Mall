using FluentValidation;

namespace Cloud_Mall.Application.Orders.Commands.UpdateStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("A valid Order ID is required.");

            // This rule ensures the incoming integer or string for the status
            // corresponds to a valid member of the VendorOrderStatus enum.
            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("The provided status is not valid. They are Pending, Processing, Shipped, Fulfilled, Cancelled");
        }
    }
}