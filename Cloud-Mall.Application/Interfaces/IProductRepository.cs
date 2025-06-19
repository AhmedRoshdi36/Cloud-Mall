using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<List<Product>> GetAllByStore(int storeId);
        Task<List<Product>> GetAllProductsAsync(string? name, string? brand, decimal? minPrice, decimal? maxPrice, double? minRate, double? maxRate, string? category, int pageNumber, int pageSize);
    }
}
