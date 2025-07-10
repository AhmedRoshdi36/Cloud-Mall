using Cloud_Mall.Application.DTOs.DeliveryOffer;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.DeliveryCompany.Query.GetVendorContactInfo
{
    public class GetVendorContactInfoQueryHandler : IRequestHandler<GetVendorContactInfoQuery, IEnumerable<VendorContactInfoDTO>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetVendorContactInfoQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<VendorContactInfoDTO>> Handle(GetVendorContactInfoQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.CustomerOrderID);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            var vendorContactInfo = order.VendorOrders.Select(vo => new VendorContactInfoDTO
            {
                VendorName = vo.Store?.Vendor?.Name,
                Phone = vo.Store?.Vendor?.PhoneNumber,
                Email = vo.Store?.Vendor?.Email,
                StoreName = vo.Store?.Name
            }).ToList();

            return vendorContactInfo;
        }
    }
} 