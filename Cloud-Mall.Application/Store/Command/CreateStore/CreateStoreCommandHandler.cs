using System.Text.Json;
using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Store.Command.CreateStore
{
    internal class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, ApiResponse<int>>
    {
        private readonly IIdentityService identity;
        private readonly ICurrentUserService currentUser;
        private readonly IStoreRepository repository;
        private readonly IFileService fileService;

        public CreateStoreCommandHandler(ICurrentUserService currentUser, IStoreRepository repository, IIdentityService identity, IFileService fileService)
        {
            this.currentUser = currentUser;
            this.repository = repository;
            this.identity = identity;
            this.fileService = fileService;
        }
        public async Task<ApiResponse<int>> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
        {
            var vendor = await identity.FindByIdAsync(currentUser.UserId);
            if (vendor == null)
            {
                return ApiResponse<int>.Failure("Vendor ID is not valid");
            }
            var addressDTOs = JsonSerializer.Deserialize<List<StoreAddressDTO>>(request.Addresses,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            string logoUrl = null;
            if (request.LogoFile != null)
            {
                logoUrl = await fileService.SaveStoreLogoAsync(request.LogoFile, currentUser.UserId);
            }


            Domain.Entities.Store store = new Domain.Entities.Store
            {
                Name = request.Name,
                LogoURL = logoUrl,
                Description = request.Description,
                StoreCategoryID = request.StoreCategoryID,
                VendorID = currentUser.UserId,
                CreatedAt = DateTime.UtcNow,
                Addresses = addressDTOs.Select(a => new StoreAddress
                {
                    StreetAddress = a.StreetAddress,
                    Notes = a.Notes,
                    GoverningLocationID = a.GoverningLocationID
                }).ToList()
            };
            await repository.AddAsync(store);
            await repository.SaveChangesAsync();

            return ApiResponse<int>.SuccessResult(store.ID);
        }
    }
}
