using FluentValidation;

namespace Cloud_Mall.Application.Products.Command.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Brand)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

            RuleFor(x => x.SKU)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Discount)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
                .When(x => x.Discount.HasValue);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be 0 or more.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Product image is required.");

            RuleFor(x => x.StoreID)
                .GreaterThan(0).WithMessage("Store ID must be greater than 0.");

            RuleFor(x => x.ProductCategoryID)
                .GreaterThan(0).WithMessage("Product category ID must be greater than 0.");
        }
    }
}
