using Cloud_Mall.Application.DTOs.Cart;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Cloud_Mall.Application.Cart.Command;

public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public RemoveProductFromCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId;
        var cart = await _unitOfWork.CartRepository.GetOrCreateCartForClientAsync(clientId);
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == request.ProductId);
        if (cartItem == null)
            return false;
        cart.CartItems.Remove(cartItem);
        _unitOfWork.CartRepository.RemoveCartItem(cartItem);
        cart.UpdatedAt = System.DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
} 