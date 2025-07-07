using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Orders.Commands.CreateOrderFromCart;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderFromCartCommandHandler : IRequestHandler<CreateOrderFromCartCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderFromCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<int>> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
        {
            var clientId = _currentUserService.UserId;
            var cart = await _unitOfWork.CartRepository.GetOrCreateCartForClientAsync(clientId);

            if (cart.CartItems is null || !cart.CartItems.Any())
            {
                return ApiResponse<int>.Failure("Your cart is empty.");
            }

            var itemsGroupedByStore = cart.CartItems.GroupBy(ci => ci.Product.StoreID).ToList();

            var customerOrder = new CustomerOrder
            {
                ClientID = clientId,
                OrderDate = DateTime.UtcNow,
                GrandTotal = 0
            };

            foreach (var storeGroup in itemsGroupedByStore)
            {
                var vendorOrder = new VendorOrder
                {
                    Status = Domain.Enums.VendorOrderStatus.Pending,
                    OrderDate = customerOrder.OrderDate,
                    ShippingCity = request.ShippingCity,
                    ShippingStreetAddress = request.ShippingStreetAddress,
                    StoreID = storeGroup.Key,
                    CustomerOrder = customerOrder,
                    SubTotal = 0
                };

                foreach (var cartItem in storeGroup)
                {
                    var orderItem = new OrderItem
                    {
                        ProductID = cartItem.ProductID,
                        Quantity = cartItem.Quantity,
                        PriceAtTimeOfPurchase = cartItem.Product.Price, // Consider using a discounted price if applicable
                        VendorOrder = vendorOrder
                    };
                    vendorOrder.OrderItems.Add(orderItem);
                    vendorOrder.SubTotal += (orderItem.PriceAtTimeOfPurchase * orderItem.Quantity);
                }
                customerOrder.VendorOrders.Add(vendorOrder);
                customerOrder.GrandTotal += vendorOrder.SubTotal;
            }

            await _unitOfWork.OrderRepository.AddCustomerOrderAsync(customerOrder);

            cart.CartItems.Clear(); // Empty the cart after checkout

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResult(customerOrder.ID);
        }
    }
}