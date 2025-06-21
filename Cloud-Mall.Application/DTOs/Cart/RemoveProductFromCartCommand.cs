using MediatR;

namespace Cloud_Mall.Application.DTOs.Cart;

public class RemoveProductFromCartCommand : IRequest<bool>
{
    public int ProductId { get; set; }
} 