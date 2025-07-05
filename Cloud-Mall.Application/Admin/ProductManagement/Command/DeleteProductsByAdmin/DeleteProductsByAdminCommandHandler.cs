using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Cloud_Mall.Application.DTOs;
using Cloud_Mall.Application.Interfaces;
using System;

namespace Cloud_Mall.Application.Admin.ProductManagement.Command.DeleteProductByAdmin
{
    public class DeleteProductsByAdminCommandHandler : IRequestHandler<DeleteProductsByAdminCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductsByAdminCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteProductsByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.ProductRepository.SoftDeleteProductsByAdminAsync(request.StoreId);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
