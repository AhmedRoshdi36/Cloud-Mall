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
            return await context.Stores.Where(s => s.IsActive)
                .Include(c => c.StoreCategory)
                .Include(s => s.Addresses)
                    .ThenInclude(a => a.GoverningLocation)
                .ToListAsync();
        }

        public async Task<List<Store>> GetAllForAdminAsync()
        {
            return await context.Stores
                .IgnoreQueryFilters()    // include soft-deleted stores for ADMMIn 
                .Include(c => c.StoreCategory)
                .Include(s => s.Addresses)
                    .ThenInclude(a => a.GoverningLocation)
                .Include(s => s.Vendor)
                .ToListAsync();
        }


        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            return await context.Stores.Where(s => s.IsActive)
                .Include(s => s.StoreCategory)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<Store>> GetStoresByCategoryNameAsync(string categoryName)
        {
            return await context.Stores.Where(s => s.IsActive)
                .Include(s => s.StoreCategory)
                .Where(s => s.StoreCategory.Name == categoryName)
                .ToListAsync();
        }

        public async Task<(List<Store> stores, int totalCount)> GetFilteredStoresAsync(
                int? categoryId,
                int? governingLocationId,
                string? streetAddress,
                int pageNumber,
                int pageSize)
        {
            var query = context.Stores
                .Where(s => s.IsActive)
                .Include(s => s.StoreCategory)
                .Include(s => s.Addresses)
                    .ThenInclude(a => a.GoverningLocation)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(s => s.StoreCategory.ID == categoryId.Value);

            if (governingLocationId.HasValue)
                query = query.Where(s => s.Addresses.Any(a => a.GoverningLocationID == governingLocationId.Value));

            if (!string.IsNullOrWhiteSpace(streetAddress))
            {
                var lowered = streetAddress.ToLower();
                query = query.Where(s => s.Addresses.Any(a => a.StreetAddress.ToLower().Contains(lowered)));
            }

            int totalCount = await query.CountAsync();

            var paginatedStores = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedStores, totalCount);
        }


        public async Task<List<Store>> GetStoresByCategoryNameByAdminAsync(string categoryName)
        {
            return await context.Stores
                .Include(s => s.StoreCategory)
                .Where(s => s.StoreCategory.Name == categoryName)
                .ToListAsync();
        }
        public async Task SoftDeleteStoreByAdminAsync(int storeId) //for Admin   
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.ID == storeId);

            if (store != null)
            {
                store.IsDeleted = true;
                store.IsActive = false;
            }
            else
                throw new ArgumentException("Store not found");


        }

        public async Task SoftDeleteStoreByVendorAsync(int storeId, string vendorId)
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.ID == storeId && s.VendorID == vendorId);
            if (store != null)
                store.IsDeleted = true;
            else
                throw new ArgumentException("Store not found or you do not have permission to delete this store");
        }

        public async Task EnableStoreByAdminAsync(int storeId) //for Admin   
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.ID == storeId);
            if (store == null)
                throw new ArgumentException("Store not found");
            else if (store.IsActive == true)
                throw new ArgumentException("Store is already active");
            else
                store.IsActive = true;


        }
        public async Task DisableStoreByAdminAsync(int storeId) //for Admin   
        {
            var store = await context.Stores.FirstOrDefaultAsync(s => s.ID == storeId);
            if (store == null)
                throw new ArgumentException("Store not found");

            store.IsActive = false;
        }


    }
}
