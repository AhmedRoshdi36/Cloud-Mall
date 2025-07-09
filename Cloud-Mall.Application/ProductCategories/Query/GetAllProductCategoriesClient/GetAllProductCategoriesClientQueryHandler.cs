using Cloud_Mall.Application.DTOs.ProductCategory;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategoriesClient;
using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Query.GetAllProductCategories
{
    public class GetAllProductCategoriesClientQueryHandler : IRequestHandler<GetAllProductCategoriesClientQuery, ApiResponse<List<ProductCategoryDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public GetAllProductCategoriesClientQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<ApiResponse<List<ProductCategoryDTO>>> Handle(GetAllProductCategoriesClientQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.ProductCategoryRepository.GetAllStoreCategories(request.storeId);

            var categoriesDTO = categories.Select(c => new ProductCategoryDTO()
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                StoreID = c.Store.ID,
            }).ToList();

            return ApiResponse<List<ProductCategoryDTO>>.SuccessResult(categoriesDTO);
        }
    }
}
