using Cloud_Mall.Application.DTOs.Cart;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Cart.Command;

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public AddProductToCartCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<bool> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(clientId))
            return false;
        await _unitOfWork.CartRepository.AddProductToCartAsync(clientId, request.ProductId, request.Quantity);
        return true;
    }
} 