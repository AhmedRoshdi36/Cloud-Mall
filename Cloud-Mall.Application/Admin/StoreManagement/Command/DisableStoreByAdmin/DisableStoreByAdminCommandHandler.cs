using Cloud_Mall.Application.Admin.StoreManagement.Command.EnableStoreByAdmin;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.StoreManagement.Command.DisableStoreByAdmin;

public class DisableStoreByAdminCommandHandler : IRequestHandler<DisableStoreByAdminCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    public DisableStoreByAdminCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> Handle(DisableStoreByAdminCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.StoresRepository.DisableStoreByAdminAsync(request.StoreId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResult(true);
        }
        catch (ArgumentException ex)
        {
            return ApiResponse<bool>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"An error occurred: {ex.Message}");
        }
    }
}

