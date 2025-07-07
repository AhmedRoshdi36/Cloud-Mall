using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Domain.Enums;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForClient
{
    public class GetCustomerOrderByIdQueryHandler : IRequestHandler<GetCustomerOrderByIdQuery, ApiResponse<CustomerOrderDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetCustomerOrderByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CustomerOrderDetailsDto>> Handle(GetCustomerOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var clientId = _currentUserService.UserId;
            var customerOrder = await _unitOfWork.OrderRepository.GetCustomerOrderByIdAsync(request.OrderId, clientId);

            if (customerOrder == null) return null;

            var dto = _mapper.Map<CustomerOrderDetailsDto>(customerOrder);
            dto.OverallStatus = CalculateOverallStatus(customerOrder.VendorOrders);

            return ApiResponse<CustomerOrderDetailsDto>.SuccessResult(dto);
        }

        private string CalculateOverallStatus(ICollection<VendorOrder> vendorOrders)
        {
            if (vendorOrders == null || !vendorOrders.Any()) return "Unknown";

            var statuses = vendorOrders.Select(vo => vo.Status).Distinct().ToList();

            if (statuses.Count == 1)
            {
                var singleStatus = statuses.First();
                return singleStatus == VendorOrderStatus.Fulfilled ? "Completed" : singleStatus.ToString();
            }
            if (statuses.All(s => s == VendorOrderStatus.Fulfilled || s == VendorOrderStatus.Cancelled))
            {
                return "Completed";
            }
            if (statuses.Any(s => s == VendorOrderStatus.Cancelled))
            {
                return "Partially Fulfilled";
            }
            return "In Progress";
        }
    }
}