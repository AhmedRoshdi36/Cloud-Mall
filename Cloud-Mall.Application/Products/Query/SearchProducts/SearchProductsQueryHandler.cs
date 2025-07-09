using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using MediatR;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, ApiResponse<GetAllProductsWithPaginationDTO>>
{
    private readonly IUnitOfWork unitOfWork;

    public SearchProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<GetAllProductsWithPaginationDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var allMatchedProducts = await unitOfWork.ProductRepository.SearchProductsAsync(
            request.StoreId,
            request.Name,
            request.Brand,
            request.Category,
            request.PageNumber,
            request.PageSize
        );

        var totalCount = allMatchedProducts.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var mapped = allMatchedProducts.Select(p => new GetAllProductsDTO
        {
            ID = p.ID,
            Name = p.Name,
            Description = p.Description,
            Brand = p.Brand,
            Price = p.Price,
            Discount = p.Discount,
            ImagesURL = p.ImagesURL,
            CategoryName = p.ProductCategory?.Name ?? "",
            AverageRate = p.Reviews.Any() ? p.Reviews.Average(r => r.Rate) : 0
        }).ToList();

        var paginatedDto = new GetAllProductsWithPaginationDTO
        {
            PageSize = request.PageSize,
            CurrentPage = request.PageNumber,
            TotalCount = totalCount,
            TotalNumberOfPages = totalPages,
            AllProducts = mapped
        };

        return ApiResponse<GetAllProductsWithPaginationDTO>.SuccessResult(paginatedDto);
    }
}
