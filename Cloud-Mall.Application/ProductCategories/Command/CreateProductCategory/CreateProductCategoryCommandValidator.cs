using Cloud_Mall.Application.ProductCategories.Command.CreateProductCategory;
using FluentValidation;

namespace Cloud_Mall.Application.ProductCategories
{
    public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
    {
        public CreateProductCategoryCommandValidator()
        {
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(p => p.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Description cannot be empty.");
            //.MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.StoreID)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Description is required.");
        }
    }
}