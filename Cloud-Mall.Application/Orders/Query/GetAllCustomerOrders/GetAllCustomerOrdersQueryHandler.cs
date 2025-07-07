using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForClient
{
    public class GetAllCustomerOrdersQueryHandler : IRequestHandler<GetAllCustomerOrdersQuery, ApiResponse<List<CustomerOrderDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAllCustomerOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CustomerOrderDetailsDto>>> Handle(GetAllCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var clientId = _currentUserService.UserId;
            var customerOrders = await _unitOfWork.OrderRepository.GetAllCustomerOrdersAsync(clientId);
            return ApiResponse<List<CustomerOrderDetailsDto>>.SuccessResult(_mapper.Map<List<CustomerOrderDetailsDto>>(customerOrders));
        }
    }
}