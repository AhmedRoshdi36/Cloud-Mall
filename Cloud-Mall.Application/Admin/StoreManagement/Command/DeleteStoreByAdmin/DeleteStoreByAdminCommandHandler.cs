using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Cloud_Mall.Application.DTOs;
using Cloud_Mall.Application.Interfaces;
using System;

namespace Cloud_Mall.Application.Admin.StoreManagement.Command.DeleteStoreByAdmin
{
    public class DeleteStoreByAdminCommandHandler : IRequestHandler<DeleteStoreByAdminCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteStoreByAdminCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteStoreByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.StoresRepository.SoftDeleteStoreByAdminAsync(request.StoreId);
                await _unitOfWork.ProductRepository.SoftDeleteProductsByAdminAsync(request.StoreId);
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
}