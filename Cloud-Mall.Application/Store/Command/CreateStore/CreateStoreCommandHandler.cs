using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Store.Command.CreateStore
{
    internal class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIdentityRepository identity;
        private readonly ICurrentUserService currentUser;
        private readonly IFileService fileService;

        public CreateStoreCommandHandler(ICurrentUserService currentUser, IIdentityRepository identity, IFileService fileService, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
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

            var cat = await unitOfWork.StoreCategoriesRepository.GetById(request.StoreCategoryID);
            if (cat == null)
            {
                return ApiResponse<int>.Failure("There is no such category for this store");
            }

            string logoUrl = null;
            if (request.LogoFile != null)
            {
                logoUrl = await fileService.SaveStoreLogoAsync(request.LogoFile, currentUser.UserId);
            }
            else
            {
                return ApiResponse<int>.Failure("You have to put a logo for your store");
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
            await unitOfWork.StoresRepository.AddAsync(store);
            await unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResult(store.ID);
        }
    }
}
