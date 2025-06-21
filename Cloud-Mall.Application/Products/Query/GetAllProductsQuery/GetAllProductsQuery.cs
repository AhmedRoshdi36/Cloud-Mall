using Cloud_Mall.Application.DTOs.Product;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetAllProductsQuery;
public class GetAllProductsQuery : IRequest<List<GetAllProductsDTO>>
{
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinRate { get; set; }
    public double? MaxRate { get; set; }
    public string? Category { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public GetAllProductsQuery(string? name = null, string? brand = null, decimal? minPrice = null, decimal? maxPrice = null, double? minRate = null, double? maxRate = null, string? category = null, int pageNumber = 1, int pageSize = 10)
    {
        Name = name;
        Brand = brand;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        MinRate = minRate;
        MaxRate = maxRate;
        Category = category;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
} 