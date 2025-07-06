using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Commands.UpdateStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _currentUserService.UserId;
            var vendorOrder = await _unitOfWork.OrderRepository.GetVendorOrderByIdAsync(request.OrderId, vendorId);

            if (vendorOrder == null)
            {
                return ApiResponse<bool>.Failure("Order not found or you do not have permission to modify it.");
            }

            // You might add business logic here, e.g., a vendor cannot change status from "Shipped" back to "Pending"
            vendorOrder.Status = request.NewStatus;

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true);
        }
    }
}