using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Command.CreateProductCategory
{
    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public CreateProductCategoryCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<ApiResponse<int>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.StoreID, currentUserService.UserId);
            if (store == null)
            {
                return ApiResponse<int>.Failure("Either you don't own the store or something went wrong");
            }
            var productCategory = new ProductCategory()
            {
                Name = request.Name,
                Description = request.Description,
                StoreID = request.StoreID,
            };
            await unitOfWork.ProductCategoryRepository.AddAsync(productCategory);
            await unitOfWork.SaveChangesAsync();
            return ApiResponse<int>.SuccessResult(productCategory.ID);
        }
    }
}
