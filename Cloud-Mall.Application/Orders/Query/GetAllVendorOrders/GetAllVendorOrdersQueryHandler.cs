using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForVendor
{
    public class GetAllVendorOrdersQueryHandler : IRequestHandler<GetAllVendorOrdersQuery, ApiResponse<List<VendorOrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllVendorOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<VendorOrderDto>>> Handle(GetAllVendorOrdersQuery request, CancellationToken cancellationToken)
        {
            var vendorId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(vendorId))
            {
                return ApiResponse<List<VendorOrderDto>>.Failure("User is not authenticated.");
            }

            var vendorOrders = await _unitOfWork.OrderRepository.GetAllOrdersForVendorAsync(vendorId);

            var dtos = _mapper.Map<List<VendorOrderDto>>(vendorOrders);

            return ApiResponse<List<VendorOrderDto>>.SuccessResult(dtos);
        }
    }
}