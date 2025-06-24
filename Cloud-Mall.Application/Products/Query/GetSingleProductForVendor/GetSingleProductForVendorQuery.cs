using Cloud_Mall.Application.DTOs.Product;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetProductForVendor
{
    public class GetSingleProductForVendorQuery : IRequest<ApiResponse<ProductDto>>
    {
        public int ProductId { get; set; }
    }
}
