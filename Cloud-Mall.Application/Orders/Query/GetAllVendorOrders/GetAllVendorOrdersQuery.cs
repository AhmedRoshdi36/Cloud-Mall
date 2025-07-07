using Cloud_Mall.Application.DTOs.Order;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForVendor
{
    // This query is simple because the vendor is identified by their authentication token.
    public class GetAllVendorOrdersQuery : IRequest<ApiResponse<List<VendorOrderDto>>>
    {
    }
}