using FluentValidation;

namespace Cloud_Mall.Application.StoreCategories.Command.CreateStoreCategory
{
    public class CreateStoreCategoryByAdminCommandValidator : AbstractValidator<CreateStoreCategoryByAdminCommand>
    {
        public CreateStoreCategoryByAdminCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(p => p.Description).NotEmpty().WithMessage("Description cannot be empty");
        }
    }
}
