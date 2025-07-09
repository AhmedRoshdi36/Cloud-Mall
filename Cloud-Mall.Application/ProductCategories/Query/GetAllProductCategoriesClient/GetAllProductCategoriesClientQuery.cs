using Cloud_Mall.Application.DTOs.ProductCategory;
using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategoriesClient
{
    public class GetAllProductCategoriesClientQuery : IRequest<ApiResponse<List<ProductCategoryDTO>>>
    {
        public int storeId { get; set; }
    }
}
