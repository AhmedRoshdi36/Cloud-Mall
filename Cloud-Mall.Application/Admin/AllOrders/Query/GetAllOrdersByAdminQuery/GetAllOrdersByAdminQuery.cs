

using Cloud_Mall.Application.DTOs.Order;
using MediatR;
using System.Globalization;

namespace Cloud_Mall.Application.Admin.AllOrders.Query.GetAllOrdersByAdminQuery;


public class GetAllOrdersByAdminQuery(String vendorId) : IRequest<ApiResponse<List<VendorOrderDto>>>
{
    public string VendorId { get; set; } = vendorId;
}
