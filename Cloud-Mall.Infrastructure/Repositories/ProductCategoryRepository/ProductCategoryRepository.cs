using Cloud_Mall.Application.Interfaces.Repositories;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext context;
        public ProductCategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Methods for vendor only
        public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
        {
            await context.ProductCategories.AddAsync(productCategory);
            return productCategory;
        }
        public async Task<List<ProductCategory>> GetAllStoreCategories(int storeId)
        {
            var categories = await context.ProductCategories
                .Include(pc => pc.Store)
                .Where(pc => pc.StoreID == storeId).ToListAsync();
            return categories;
        }

        public async Task<ProductCategory> GetById(int id, int storeId)
        {
            return await context.ProductCategories.FirstOrDefaultAsync(p => p.StoreID == storeId && p.ID == id);
        }
    }
}
