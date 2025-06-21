using Cloud_Mall.Application.DTOs.Cart;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Cloud_Mall.Application.Cart.Command;

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId;
        var cart = await _unitOfWork.CartRepository.GetOrCreateCartForClientAsync(clientId);
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == request.ProductId);
        if (cartItem == null)
            return false;
        // Assume ProductRepository.GetProductByIdAsync is available
        var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(request.ProductId);
        if (product == null || request.Quantity < 1 || request.Quantity > product.Stock)
            return false;
        cartItem.Quantity = request.Quantity;
        cart.UpdatedAt = System.DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
} 