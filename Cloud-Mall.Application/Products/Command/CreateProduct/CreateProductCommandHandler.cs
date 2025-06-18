using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<ApiResponse<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.StoreID, currentUserService.UserId);
            if (store == null)
            {
                return ApiResponse<int>.Failure("Either you dont own the store or smth went wrong");
            }

            var category = await unitOfWork.ProductCategoryRepository.GetById(request.ProductCategoryID, request.StoreID);
            if (category == null)
            {
                return ApiResponse<int>.Failure("There is no such category for this store");
            }

            string logoUrl = null;
            if (request.Image != null)
            {
                logoUrl = await fileService.SaveProductImageAsync(request.Image, currentUserService.UserId);
            }
            else
            {
                return ApiResponse<int>.Failure("You have to put a logo for your product");
            }

            var product = new Product()
            {
                Name = request.Name,
                ImagesURL = logoUrl,
                Description = request.Description,
                Brand = request.Brand,
                SKU = request.SKU,
                Price = request.Price,
                Stock = request.Stock,
                StoreID = request.StoreID,
                ProductCategoryID = request.ProductCategoryID,
            };

            await unitOfWork.ProductRepository.AddProduct(product);
            await unitOfWork.SaveChangesAsync();
            return ApiResponse<int>.SuccessResult(product.ID);
        }
    }
}
