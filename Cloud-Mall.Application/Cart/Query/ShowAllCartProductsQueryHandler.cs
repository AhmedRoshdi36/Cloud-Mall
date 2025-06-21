using Cloud_Mall.Application.DTOs.Cart;
using Cloud_Mall.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Cart.Query;

public class ShowAllCartProductsQueryHandler : IRequestHandler<ShowAllCartProductsQuery, List<CartProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public ShowAllCartProductsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<List<CartProductDTO>> Handle(ShowAllCartProductsQuery request, CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId;
        var cart = await _unitOfWork.CartRepository.GetOrCreateCartForClientAsync(clientId);
        var products = cart.CartItems.Select(ci => new CartProductDTO
        {
            ProductId = ci.ProductID,
            Name = ci.Product.Name,
            ImageURL = ci.Product.ImagesURL,
            Price = ci.Product.Price,
            Discount = ci.Product.Discount,
            StoreName = ci.Product.Store.Name,
            Quantity = ci.Quantity
        }).ToList();
        return products;
    }
} 