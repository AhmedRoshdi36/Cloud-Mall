using MediatR;

namespace Cloud_Mall.Application.Orders.Commands.CreateOrderFromCart
{
    public class CreateOrderFromCartCommand : IRequest<ApiResponse<int>>
    {
        public string ShippingCity { get; set; }
        public string ShippingStreetAddress { get; set; }
    }

}
