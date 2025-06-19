using MediatR;

namespace Cloud_Mall.Application.DTOs.Cart;

public class UpdateCartItemQuantityCommand : IRequest<bool>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
} 