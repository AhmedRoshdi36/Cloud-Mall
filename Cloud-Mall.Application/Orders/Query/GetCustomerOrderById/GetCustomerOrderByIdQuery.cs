using Cloud_Mall.Application.DTOs.Order;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForClient
{
    public class GetCustomerOrderByIdQuery : IRequest<ApiResponse<CustomerOrderDetailsDto>>
    {
        public int OrderId { get; set; }
    }
}