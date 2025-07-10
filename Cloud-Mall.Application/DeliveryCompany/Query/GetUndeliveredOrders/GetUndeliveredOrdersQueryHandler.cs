using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.DeliveryCompany.Query.GetUndeliveredOrders
{
    public class GetUndeliveredOrdersQueryHandler : IRequestHandler<GetUndeliveredOrdersQuery, IEnumerable<CustomerOrderDetailsDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetUndeliveredOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<CustomerOrderDetailsDto>> Handle(GetUndeliveredOrdersQuery request, CancellationToken cancellationToken)
        {
            var undeliveredOrders = await _orderRepository.GetUndeliveredOrdersAsync();
            
            return undeliveredOrders.Select(order => new CustomerOrderDetailsDto
            {
                Id = order.ID,
                GrandTotal = order.GrandTotal,
                OrderDate = order.OrderDate,
                OverallStatus = "Undelivered", // or map from order if available
                VendorOrders = order.VendorOrders.Select(vo => new VendorOrderDto
                {
                    Id = vo.ID,
                    CustomerOrderId = vo.CustomerOrderID,
                    StoreName = vo.Store?.Name,
                    OrderDate = vo.OrderDate.ToString("yyyy-MM-dd"),
                    SubTotal = vo.SubTotal,
                    Status = vo.Status,
                    ClientName = order.Client?.Name,
                    ShippingAddress = $"{vo.ShippingCity}, {vo.ShippingStreetAddress}",
                    OrderItems = null // or map if needed
                }).ToList()
            });
        }
    }
} 