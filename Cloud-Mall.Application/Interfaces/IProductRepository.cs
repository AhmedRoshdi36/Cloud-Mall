using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<List<Product>> GetAllByStore(int storeId);
    }
}
