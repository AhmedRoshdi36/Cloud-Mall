using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<List<Product>> GetAllByStore(int storeId);
        Task<(List<Product> Products, int TotalCount)> GetAllProductsAsync(int storeId, string? name, string? brand, decimal? minPrice, decimal? maxPrice, double? minRate, double? maxRate, string? category, int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(int id);
        Task SoftDeleteProductByAdminAsync(int productId); //for Admin
        Task SoftDeleteProductByVendorAsync(int productId, string vendorId); //check vendor
        Task SoftDeleteProductsByAdminAsync(int storeId);   //for Admin 
        Task SoftDeleteProductsByVendorAsync(int storeId, string vendorId); //check vendor

        public Task<List<Product>> SearchProductsAsync(
            int storeId,
            string? name,
            string? brand,
            string? category,
            int pageNumber,
            int pageSize);
    }
}
