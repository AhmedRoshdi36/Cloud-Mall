using Cloud_Mall.Application.DTOs.Product;
using MediatR;

namespace Cloud_Mall.Application.Admin.ProductManagement.Query.GetAllProductsForStoreByAdmin;

public class GetAllProductsForStoreByAdminQuery : IRequest<ApiResponse<List<ProductDto>>>
{
    public int StoreId { get; set; }
}
