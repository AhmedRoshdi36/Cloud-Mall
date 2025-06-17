using Cloud_Mall.Application.DTOs.ProductCategory;
using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories
{
    public class GetAllProductCategoriesQuery : IRequest<ApiResponse<List<ProductCategoryDTO>>>
    {
        public int storeId { get; set; }
    }
}
