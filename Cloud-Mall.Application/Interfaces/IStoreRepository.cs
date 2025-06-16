namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreRepository
    {
        Task AddAsync(Domain.Entities.Store store);
        Task<List<Domain.Entities.Store>> GetAllByVendorAsync(string vendorId);
        Task<Domain.Entities.Store?> GetByIdAsync(int id, string vendorId);
        Task DeleteAsync(Domain.Entities.Store store);
    }
}
