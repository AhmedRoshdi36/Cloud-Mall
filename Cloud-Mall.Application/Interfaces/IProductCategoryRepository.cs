using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> AddAsync(ProductCategory productCategory);
    }
}