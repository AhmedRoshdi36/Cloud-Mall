using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForVendor
{
    // Update the return type here
    public class GetAllOrdersForStoreQueryHandler : IRequestHandler<GetAllOrdersForStoreQuery, ApiResponse<VendorStoreOrdersResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllOrdersForStoreQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<VendorStoreOrdersResponseDTO>> Handle(GetAllOrdersForStoreQuery request, CancellationToken cancellationToken)
        {
            var vendorId = _currentUserService.UserId;
            var vendorOrders = await _unitOfWork.OrderRepository.GetAllOrdersForStoreAsync(request.StoreId, vendorId);

            if (vendorOrders == null)
            {
                return ApiResponse<VendorStoreOrdersResponseDTO>.Failure("Store not found or you do not have permission to view its orders.");
            }

            // Create the new response object
            var response = new VendorStoreOrdersResponseDTO
            {
                TotalOrders = vendorOrders.Count(),
                Orders = _mapper.Map<List<VendorOrderDto>>(vendorOrders)
            };

            return ApiResponse<VendorStoreOrdersResponseDTO>.SuccessResult(response);
        }
    }
}