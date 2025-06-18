using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddProduct(Product product)
        {
            await context.Products.AddAsync(product);
        }

        public async Task<List<Product>> GetAllByStore(int storeId)
        {
            return await context.Products
                .Where(p => p.StoreID == storeId)
                .ToListAsync();
        }
    }
}
