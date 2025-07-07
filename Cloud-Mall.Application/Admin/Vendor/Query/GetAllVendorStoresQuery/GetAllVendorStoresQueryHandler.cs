using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;

internal class GetAllVendorStoresByAdminQueryHandler : IRequestHandler<GetAllVendorStoresByAdminQuery, ApiResponse<List<GetOneStoreDTO>>>
{
    private readonly IUnitOfWork unitOfWork;
    public GetAllVendorStoresByAdminQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        //this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
    }
    public async Task<ApiResponse<List<GetOneStoreDTO>>> Handle(GetAllVendorStoresByAdminQuery request, CancellationToken cancellationToken)
    {
        //var stores = await unitOfWork.StoresRepository.GetAllByVendorAsync(currentUserService.UserId);
        var stores = await unitOfWork.StoresRepository.GetAllByVendorAsync(request.VendorId);
        if (stores == null)
        {
            return ApiResponse<List<GetOneStoreDTO>>.Failure("This vendor has no stores");
        }
        var storesDto = stores.Select(s => new GetOneStoreDTO
        {
            ID = s.ID,
            Name = s.Name,
            Description = s.Description,
            LogoURL = s.LogoURL,
            CategoryName = s.StoreCategory.Name,
            Addresses = s.Addresses.Select(a => new GetStoreAddressDTO
            {
                StreetAddress = a.StreetAddress,
                Notes = a.Notes,
                GoverningLocationName = a.GoverningLocation?.Name
            }).ToList()
        }).ToList();

        return ApiResponse<List<GetOneStoreDTO>>.SuccessResult(storesDto);
    }
}
