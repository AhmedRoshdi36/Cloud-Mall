using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetOrCreateCartForClientAsync(string clientId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.Store)
                .FirstOrDefaultAsync(c => c.ClientID == clientId);
            if (cart == null)
            {
                cart = new Cart { ClientID = clientId, UpdatedAt = DateTime.UtcNow };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task AddProductToCartAsync(string clientId, int productId, int quantity)
        {
            var cart = await GetOrCreateCartForClientAsync(clientId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }//commandHandler ?
            else
            {
                cartItem = new CartItem { ProductID = productId, Quantity = quantity, CartID = cart.ID };
                cart.CartItems.Add(cartItem);
                _context.CartItems.Add(cartItem);
            }
            cart.UpdatedAt = DateTime.UtcNow;//search?
            await _context.SaveChangesAsync();
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}