using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> AddAsync(ProductCategory productCategory);
        Task<List<ProductCategory>> GetAllStoreCategories(int storeId);
        Task<ProductCategory> GetById(int id, int storeId);
    }
}