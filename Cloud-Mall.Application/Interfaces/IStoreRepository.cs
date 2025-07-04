using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreRepository
    {
        Task AddAsync(Store store);
        Task<List<Store>> GetAllByVendorAsync(string vendorId);
        Task<Store?> GetByIdAsync(int id, string vendorId);
        Task<List<Store>> GetAllAsync();
        Task<Store?> GetStoreByIdAsync(int id);
        Task<List<Store>> GetStoresByCategoryNameAsync(string categoryName);
        Task SoftDeleteStoreByAdminAsync(int storetId); //for Admin
        Task SoftDeleteStoreByVendorAsync(int storetId, string vendorId); //check veendor
        

    }
}
