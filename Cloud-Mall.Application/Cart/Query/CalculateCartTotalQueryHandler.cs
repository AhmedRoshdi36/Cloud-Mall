using Cloud_Mall.Application.DTOs.Cart;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Cart.Query;

public class CalculateCartTotalQueryHandler : IRequestHandler<CalculateCartTotalQuery, CartTotalDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public CalculateCartTotalQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<CartTotalDTO> Handle(CalculateCartTotalQuery request, CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId;
        var cart = await _unitOfWork.CartRepository.GetOrCreateCartForClientAsync(clientId);
        var totalBefore = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
        var totalAfter = cart.CartItems.Sum(ci =>
        {
            var price = ci.Product.Price;
            if (ci.Product.Discount.HasValue && ci.Product.Discount.Value > 0)
                price -= ci.Product.Discount.Value;
            return price * ci.Quantity;
        });
        return new CartTotalDTO
        {
            TotalBeforeDiscount = totalBefore,
            TotalAfterDiscount = totalAfter
        };
    }
} 