using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories.StoreRepository
{
    internal class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext context;
        public StoreRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Store store)
        {
            await context.AddAsync(store);
        }

        public async Task DeleteAsync(Store store)
        {
            context.Stores.Remove(store);
        }

        public async Task<List<Store>> GetAllByVendorAsync(string vendorId)
        {
            return await context.Stores.Where(s => s.VendorID == vendorId)
                .Include(s => s.StoreCategory)
                .Include(s => s.Addresses)
                    .ThenInclude(a => a.GoverningLocation)
                .ToListAsync();
        }

        public async Task<Store?> GetByIdAsync(int id, string vendorId)
        {
            return await context.Stores
                        .Include(s => s.Addresses)
                            .ThenInclude(a => a.GoverningLocation)
                        .Include(s => s.StoreCategory)
                        .FirstOrDefaultAsync(s => s.ID == id && s.VendorID == vendorId);
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await context.Stores
                .Include(c => c.StoreCategory)
                .Include(s => s.Addresses)
                    .ThenInclude(a => a.GoverningLocation)
                .ToListAsync();
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            return await context.Stores
                .Include(s => s.StoreCategory)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<Store>> GetStoresByCategoryNameAsync(string categoryName)
        {
            return await context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.StoreCategory.Name == categoryName)
                .ToListAsync();
        }
    }
}
