using Cloud_Mall.Application.Admin.ProductManagement.Command.DeleteProductByAdmin;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.ProductManagement.Command.DeleteProductByAdmin
{
    public class DeleteProductByAdminCommandHandler(IUnitOfWork unitOfWork) 
        : IRequestHandler<DeleteProductByAdminCommand, ApiResponse<bool>>

    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ApiResponse<bool>> Handle(DeleteProductByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.ProductRepository.SoftDeleteProductByAdminAsync(request.ProductId);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true);
            }
            catch (KeyNotFoundException ex)
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





