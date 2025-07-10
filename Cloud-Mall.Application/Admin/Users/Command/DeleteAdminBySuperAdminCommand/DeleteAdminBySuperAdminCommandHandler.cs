
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Admin.Users.Command.DeleteAdminBySuperAdminCommand;

internal class DeleteAdminBySuperAdminCommandHandler(IIdentityRepository identityRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteAdminBySuperAdminCommand, ApiResponse<bool>>
{


    public async Task<ApiResponse<bool>> Handle(DeleteAdminBySuperAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await identityRepository.FindByIdAsync(request.AdminId);
        if (admin == null)
        {
            return ApiResponse<bool>.Failure("This user is not an admin or does not exist.");
        }
        var deleted = await identityRepository.DeleteAdminByIdAsync(request.AdminId);
        if (!deleted)
        {
            return ApiResponse<bool>.Failure("Failed to delete admin.");
        }

        await unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResult(true);
    }
}
