using Cloud_Mall.Application.DTOs.Product;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Products.Query.GetProductForVendor;
using MediatR;

namespace Cloud_Mall.Application.Products.Query.GetSingleProductForVendor
{
    public class GetSingleProductForVendorQueryHandler : IRequestHandler<GetSingleProductForVendorQuery, ApiResponse<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public GetSingleProductForVendorQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<ApiResponse<ProductDto>> Handle(GetSingleProductForVendorQuery request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.ProductRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                return ApiResponse<ProductDto>.Failure("Product not found");
            }

            var productDto = new ProductDto()
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                SKU = product.SKU,
                Price = product.Price,
                Discount = product.Discount,
                Stock = product.Stock,
                ImagesURL = product.ImagesURL,
                StoreID = product.StoreID,
                StoreName = product.Store.Name,
                ProductCategoryID = product.ProductCategoryID,
                ProductCategoryName = product.ProductCategory.Name,
                AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rate) : 0,
                ReviewCount = product.Reviews.Count
            };

            return ApiResponse<ProductDto>.SuccessResult(productDto);
        }
    }
}
