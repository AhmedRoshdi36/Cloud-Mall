using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories
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
                .Include(s => s.Addresses)
                .ToListAsync();
        }

        public async Task<Store?> GetByIdAsync(int id, string vendorId)
        {
            return await context.Stores
                        .Include(s => s.Addresses)
                        .FirstOrDefaultAsync(s => s.ID == id && s.VendorID == vendorId);
        }
    }
}
