using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreRepository
    {
        Task AddAsync(Store store);
        Task<List<Store>> GetAllByVendorAsync(string vendorId);
        Task<Store?> GetByIdAsync(int id, string vendorId);
        Task DeleteAsync(Store store);
    }
}
