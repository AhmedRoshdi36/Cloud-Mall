using Cloud_Mall.Application.DTOs.Order;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForClient
{
    public class GetAllCustomerOrdersQuery : IRequest<ApiResponse<List<CustomerOrderDetailsDto>>>
    {
    }
}