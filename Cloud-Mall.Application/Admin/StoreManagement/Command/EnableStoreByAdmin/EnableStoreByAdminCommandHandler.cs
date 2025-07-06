using Cloud_Mall.Application.Interfaces;
using MediatR;


namespace Cloud_Mall.Application.Admin.StoreManagement.Command.EnableStoreByAdmin;
    public class EnableStoreByAdminCommandHandler : IRequestHandler<EnableStoreByAdminCommand, ApiResponse<bool>>
    {
    private readonly IUnitOfWork _unitOfWork;
    public EnableStoreByAdminCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

        public async Task<ApiResponse<bool>> Handle(EnableStoreByAdminCommand request, CancellationToken cancellationToken)
        {
               try
               {
                   await _unitOfWork.StoresRepository.EnableStoreByAdminAsync(request.StoreId);
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
