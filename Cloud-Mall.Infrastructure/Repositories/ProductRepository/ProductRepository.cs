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
                .Include(p => p.ProductCategory)
                .Include(p => p.Store)
                .Include(p => p.Reviews)
                .Where(p => p.StoreID == storeId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync(string? name, string? brand, decimal? minPrice, decimal? maxPrice, double? minRate, double? maxRate, string? category, int pageNumber, int pageSize)
        {
            var query = context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Reviews)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));
            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand.Contains(brand));
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.ProductCategory.Name == category);
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);
            if (minRate.HasValue)
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rate) >= minRate.Value);
            if (maxRate.HasValue)
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rate) <= maxRate.Value);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Reviews)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.ID == id);
        }
    }
}
