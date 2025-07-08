using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Admin.ProductManagement.Query.GetAllProductsForStoreByAdmin;

public class GetAllProductsForStoreByAdminQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllProductsForStoreByAdminQuery, ApiResponse<List<ProductDto>>>
  {
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ApiResponse<List<ProductDto>>> Handle(GetAllProductsForStoreByAdminQuery request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.GetAllByStore(request.StoreId);
        var productsDto = products.Select(p => new ProductDto
        {
            ID = p.ID,
            Name = p.Name,
            Description = p.Description,
            Brand = p.Brand,
            SKU = p.SKU,
            Price = p.Price,
            Discount = p.Discount,
            Stock = p.Stock,
            ImagesURL = p.ImagesURL,
            StoreID = p.StoreID,
            StoreName = p.Store.Name,
            ProductCategoryID = p.ProductCategoryID,
            ProductCategoryName = p.ProductCategory.Name,
            AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rate) : 0,
            ReviewCount = p.Reviews.Count
        }).ToList();

        return ApiResponse<List<ProductDto>>.SuccessResult(productsDto);
    }
}
