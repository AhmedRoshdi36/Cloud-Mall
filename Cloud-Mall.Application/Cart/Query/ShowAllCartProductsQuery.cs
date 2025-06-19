using Cloud_Mall.Application.DTOs.Cart;
using MediatR;
using System.Collections.Generic;

namespace Cloud_Mall.Application.Cart.Query;

public class ShowAllCartProductsQuery : IRequest<List<CartProductDTO>>
{
} 