using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.Orders.Queries.GetForClient
{
    // Update the return type here
    public class GetAllCustomerOrdersQueryHandler : IRequestHandler<GetAllCustomerOrdersQuery, ApiResponse<ClientOrdersResponseDTO>>
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

        public async Task<ApiResponse<ClientOrdersResponseDTO>> Handle(GetAllCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var clientId = _currentUserService.UserId;
            var customerOrders = await _unitOfWork.OrderRepository.GetAllCustomerOrdersAsync(clientId);

            // Create the new response object
            var response = new ClientOrdersResponseDTO
            {
                // Get the total count from the list
                TotalOrders = customerOrders?.Count() ?? 0,
                // Map the list of entities to a list of DTOs
                Orders = _mapper.Map<List<CustomerOrderDetailsDto>>(customerOrders)
            };

            return ApiResponse<ClientOrdersResponseDTO>.SuccessResult(response);
        }
    }
}