
using AutoMapper;
using Cloud_Mall.Application.DTOs.Order;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Orders.Queries.GetForVendor;
using MediatR;

namespace Cloud_Mall.Application.Admin.AllOrders.Query.GetAllOrdersByAdminQuery;

internal class GetAllOrdersByAdminQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper) : IRequestHandler<GetAllOrdersByAdminQuery, ApiResponse<List<VendorOrderDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<List<VendorOrderDto>>> Handle(GetAllOrdersByAdminQuery request, CancellationToken cancellationToken)
    {
        var vendorId = request.VendorId;

        var vendorOrders = await _unitOfWork.OrderRepository.GetAllOrdersForVendorAsync(vendorId);

        var dtos = _mapper.Map<List<VendorOrderDto>>(vendorOrders);

        return ApiResponse<List<VendorOrderDto>>.SuccessResult(dtos);
    }
}


