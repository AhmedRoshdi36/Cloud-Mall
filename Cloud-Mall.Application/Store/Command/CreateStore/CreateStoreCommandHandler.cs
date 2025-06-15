using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Store.Command.CreateStore
{
    internal class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, ApiResponse<int>>
    {
        private readonly IIdentityRepository identity;
        private readonly ICurrentUserService currentUser;
        private readonly IStoreRepository storeRepository;
        private readonly IFileService fileService;
        private readonly IStoreCategoryRepository storeCategoryRepository;

        public CreateStoreCommandHandler(ICurrentUserService currentUser, IStoreRepository storeRepository, IIdentityRepository identity, IFileService fileService, IStoreCategoryRepository storeCategoryRepository)
        {
            this.currentUser = currentUser;
            this.storeRepository = storeRepository;
            this.identity = identity;
            this.fileService = fileService;
            this.storeCategoryRepository = storeCategoryRepository;
        }
        public async Task<ApiResponse<int>> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
        {
            var vendor = await identity.FindByIdAsync(currentUser.UserId);
            if (vendor == null)
            {
                return ApiResponse<int>.Failure("Vendor ID is not valid");
            }

            var cat = await storeCategoryRepository.GetById(request.StoreCategoryID);
            if (cat == null)
            {
                return ApiResponse<int>.Failure("There is no such category for this store");
            }

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
            };
            await storeRepository.AddAsync(store);
            await storeRepository.SaveChangesAsync();

            return ApiResponse<int>.SuccessResult(store.ID);
        }
    }
}
