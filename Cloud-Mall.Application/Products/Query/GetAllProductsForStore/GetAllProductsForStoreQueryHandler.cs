using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Products.Query.GetAllProducts;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetAllProductsForStore
{
    public class GetAllProductsForStoreQueryHandler : IRequestHandler<GetAllProductsForStoreQuery, ApiResponse<List<ProductDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllProductsForStoreQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<ApiResponse<List<ProductDto>>> Handle(GetAllProductsForStoreQuery request, CancellationToken cancellationToken)
        {
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.StoreId, currentUserService.UserId);
            if (store == null)
            {
                return ApiResponse<List<ProductDto>>.Failure("Either you dont own the store or smth went wrong");
            }

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
                StoreName = p.Store?.Name,
                ProductCategoryID = p.ProductCategoryID,
                ProductCategoryName = p.ProductCategory.Name,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rate) : 0,
                ReviewCount = p.Reviews.Count
            }).ToList();

            return ApiResponse<List<ProductDto>>.SuccessResult(productsDto);
        }
    }
}
