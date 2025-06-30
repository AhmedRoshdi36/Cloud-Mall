using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetVendorStoreById
{
    public class GetVendorStoreByIdQueryHandler : IRequestHandler<GetVendorStoreByIdQuery, ApiResponse<StoreDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetVendorStoreByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<ApiResponse<StoreDTO>> Handle(GetVendorStoreByIdQuery request, CancellationToken cancellationToken)
        {
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.StoreId, currentUserService.UserId);
            if (store == null)
            {
                return ApiResponse<StoreDTO>.Failure("Either you dont own this store or smth wrong happened");
            }
            var storeDto = new StoreDTO()
            {
                ID = store.ID,
                Name = store.Name,
                Description = store.Description,
                LogoURL = store.LogoURL,
                CategoryName = store.StoreCategory.Name,
                Addresses = store.Addresses.Select(a => new GetStoreAddressDTO()
                {
                    StreetAddress = a.StreetAddress,
                    Notes = a.Notes,
                    GoverningLocationName = a.GoverningLocation.Name,
                }).ToList(),
            };

            return ApiResponse<StoreDTO>.SuccessResult(storeDto);
        }
    }
}
