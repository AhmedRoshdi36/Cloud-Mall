using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.StoreCategories.Command.CreateStoreCategory
{
    public class CreateStoreCategoryByAdminCommandHandler : IRequestHandler<CreateStoreCategoryByAdminCommand, ApiResponse<StoreCategoryDto>>
    {
        private readonly IStoreCategoryRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateStoreCategoryByAdminCommandHandler(IStoreCategoryRepository _repository, IUnitOfWork unitOfWork)
        {
            this.repository = _repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<StoreCategoryDto>> Handle(CreateStoreCategoryByAdminCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.StoreCategoriesRepository.CreateAsync(request.Name, request.Description);
            await unitOfWork.SaveChangesAsync();
            var categoryDto = new StoreCategoryDto()
            {
                Id = category.ID,
                Name = category.Name,
                Description = category.Description,
            };
            return ApiResponse<StoreCategoryDto>.SuccessResult(categoryDto);
        }

        
    }
}
