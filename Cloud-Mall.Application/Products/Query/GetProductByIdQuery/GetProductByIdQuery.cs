using Cloud_Mall.Application.DTOs.Product;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetProductByIdQuery;
public class GetProductByIdQuery : IRequest<GetProductByIdDTO>
{
    public int ProductId { get; set; }
    public GetProductByIdQuery(int productId)
    {
        ProductId = productId;
    }
} 