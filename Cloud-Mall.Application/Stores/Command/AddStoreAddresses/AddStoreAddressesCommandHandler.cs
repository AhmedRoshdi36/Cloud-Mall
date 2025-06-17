using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Stores.Command.AddStoreAddresses
{
    public class AddStoreAddressesCommandHandler : IRequestHandler<AddStoreAddressesCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public AddStoreAddressesCommandHandler(IStoreRepository storeRepository, ICurrentUserService currentUser, IGoverningLocationRepository governingLocationRepository, IUnitOfWork unitOfWork)
        {
            this.currentUser = currentUser;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<bool>> Handle(AddStoreAddressesCommand request, CancellationToken cancellationToken)
        {
            var locationIds = request.Addresses.Select(a => a.GoverningLocationID).Distinct();
            var locationsExist = await unitOfWork.GoverningLocationsRepository.AllExistAsync(locationIds);

            if (!locationsExist)
            {
                return ApiResponse<bool>.Failure("One or more governing location IDs are invalid.");
            }
            var store = await unitOfWork.StoresRepository.GetByIdAsync(request.StoreId, currentUser.UserId);
            if (store == null)
            {
                return ApiResponse<bool>.Failure("Either store not found or You are not the vendor of this store.");
            }

            var newAddresses = request.Addresses.Select(a => new StoreAddress
            {
                StreetAddress = a.StreetAddress,
                Notes = a.Notes,
                GoverningLocationID = a.GoverningLocationID,
                StoreID = store.ID,
            }).ToList();

            foreach (var address in newAddresses)
            {
                store.Addresses.Add(address);
            }
            await unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResult(true);
        }
    }
}
