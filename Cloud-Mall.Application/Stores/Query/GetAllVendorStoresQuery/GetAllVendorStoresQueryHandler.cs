using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery
{
    internal class GetAllVendorStoresQueryHandler : IRequestHandler<GetAllVendorStoresQuery, ApiResponse<List<GetOneStoreDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        public GetAllVendorStoresQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            this.currentUserService = currentUserService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<List<GetOneStoreDTO>>> Handle(GetAllVendorStoresQuery request, CancellationToken cancellationToken)
        {
            var stores = await unitOfWork.StoresRepository.GetAllByVendorAsync(currentUserService.UserId);
            if (stores == null)
            {
                return ApiResponse<List<GetOneStoreDTO>>.Failure("This vendor has no stores");
            }
            var storesDto = stores.Select(s => new GetOneStoreDTO()
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                LogoURL = s.LogoURL,
                CategoryName = s.StoreCategory.Name,
            }).ToList();

            return ApiResponse<List<GetOneStoreDTO>>.SuccessResult(storesDto);
        }
    }
}
