using FluentValidation;

namespace Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories
{
    public class GetAllProductCategoriesQueryValidator : AbstractValidator<GetAllProductCategoriesQuery>
    {
        public GetAllProductCategoriesQueryValidator()
        {
            RuleFor(p => p.storeId).NotEmpty().WithMessage("Store ID cannot be empty");
        }
    }
}
