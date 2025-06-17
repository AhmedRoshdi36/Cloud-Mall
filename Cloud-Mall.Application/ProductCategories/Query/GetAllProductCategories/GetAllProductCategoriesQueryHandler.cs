using Cloud_Mall.Application.DTOs.ProductCategory;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories
{
    public class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, ApiResponse<List<ProductCategoryDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public GetAllProductCategoriesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<ApiResponse<List<ProductCategoryDTO>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.storeId, currentUserService.UserId);
            if (store == null)
            {
                return ApiResponse<List<ProductCategoryDTO>>.Failure("You dont own this store!");
            }

            var categories = await unitOfWork.ProductCategoryRepository.GetAllStoreCategories(request.storeId);

            var categoriesDTO = categories.Select(c => new ProductCategoryDTO()
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                StoreID = store.ID,
            }).ToList();

            return ApiResponse<List<ProductCategoryDTO>>.SuccessResult(categoriesDTO);
        }
    }
}
