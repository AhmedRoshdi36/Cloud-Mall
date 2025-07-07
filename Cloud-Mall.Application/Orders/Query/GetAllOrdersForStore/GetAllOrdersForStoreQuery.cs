using Cloud_Mall.Application.DTOs.Order;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForVendor
{
    public class GetAllOrdersForStoreQuery : IRequest<ApiResponse<List<VendorOrderDto>>>
    {
        public int StoreId { get; set; }
    }
}