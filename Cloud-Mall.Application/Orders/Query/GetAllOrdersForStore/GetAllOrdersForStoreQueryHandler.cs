using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForVendor
{
    public class GetAllOrdersForStoreQueryHandler : IRequestHandler<GetAllOrdersForStoreQuery, ApiResponse<List<VendorOrderDto>>>
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

        public async Task<ApiResponse<List<VendorOrderDto>>> Handle(GetAllOrdersForStoreQuery request, CancellationToken cancellationToken)
        {
            var vendorId = _currentUserService.UserId;
            var vendorOrders = await _unitOfWork.OrderRepository.GetAllOrdersForStoreAsync(request.StoreId, vendorId);

            if (vendorOrders == null)
            {
                return ApiResponse<List<VendorOrderDto>>.Failure("Store not found or you do not have permission to view its orders.");
            }

            var dtos = _mapper.Map<List<VendorOrderDto>>(vendorOrders);
            return ApiResponse<List<VendorOrderDto>>.SuccessResult(dtos);
        }
    }
}