using Cloud_Mall.Application.Interfaces.Repositories;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;

namespace Cloud_Mall.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext context;
        public ProductCategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
        {
            await context.ProductCategories.AddAsync(productCategory);
            return productCategory;
        }
    }
}
