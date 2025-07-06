using Cloud_Mall.Domain.Enums;
using MediatR;

namespace Cloud_Mall.Application.Orders.Commands.UpdateStatus
{
    public class UpdateOrderStatusCommand : IRequest<ApiResponse<bool>>
    {
        public int OrderId { get; set; }
        public VendorOrderStatus NewStatus { get; set; }
    }
}