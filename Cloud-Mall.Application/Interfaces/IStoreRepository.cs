using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreRepository
    {
        Task AddAsync(Store store);
        Task<List<Store>> GetAllByVendorAsync(string vendorId);
        Task<Store?> GetByIdAsync(int id, string vendorId);
        Task<List<Store>> GetAllAsync();
        Task<List<Store>> GetAllForAdminAsync();
        Task<Store?> GetStoreByIdAsync(int id);
        Task<List<Store>> GetStoresByCategoryNameAsync(string categoryName);
        Task<List<Store>> GetStoresByCategoryNameByAdminAsync(string categoryName);
        Task<(List<Store> stores, int totalCount)> GetFilteredStoresAsync(
            int? categoryId,
            int? governingLocationId,
            string? streetAddress,
            int pageNumber,
            int pageSize);

        Task SoftDeleteStoreByAdminAsync(int storetId); //for Admin
        Task SoftDeleteStoreByVendorAsync(int storetId, string vendorId); //check veendor
        Task EnableStoreByAdminAsync(int storeId);
        Task DisableStoreByAdminAsync(int storeId);



    }
}
