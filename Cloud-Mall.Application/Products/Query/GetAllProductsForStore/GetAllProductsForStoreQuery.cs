using Cloud_Mall.Application.DTOs.Product;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetAllProducts
{
    public class GetAllProductsForStoreQuery : IRequest<ApiResponse<List<ProductDto>>>
    {
        public int StoreId { get; set; }
    }
}
