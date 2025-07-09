using Cloud_Mall.Application.DTOs.Product;
using MediatR;

public class SearchProductsQuery : IRequest<ApiResponse<GetAllProductsWithPaginationDTO>>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
