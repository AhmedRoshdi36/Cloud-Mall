using Cloud_Mall.Application.DTOs.Order;
using MediatR;

namespace Cloud_Mall.Application.DeliveryCompany.Query.GetUndeliveredOrders
{
    public class GetUndeliveredOrdersQuery : IRequest<IEnumerable<CustomerOrderDetailsDto>>
    {
        // No parameters needed - returns all undelivered orders
    }
} 