using Cloud_Mall.Domain.Entities;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cloud_Mall.Domain.Entities.Cart> GetOrCreateCartForClientAsync(string clientId);
        Task AddProductToCartAsync(string clientId, int productId, int quantity);
    }
} 