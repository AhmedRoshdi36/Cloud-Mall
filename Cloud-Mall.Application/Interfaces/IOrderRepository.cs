using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddCustomerOrderAsync(CustomerOrder order);

        // For Clients
        Task<CustomerOrder?> GetCustomerOrderByIdAsync(int orderId, string clientId);
        Task<IEnumerable<CustomerOrder>?> GetAllCustomerOrdersAsync(string clientId);

        // For Vendors
        Task<VendorOrder?> GetVendorOrderByIdAsync(int orderId, string vendorId);
        Task<IEnumerable<VendorOrder>?> GetAllOrdersForVendorAsync(string vendorId);

        // --- THIS IS THE NEW, CORRECTED METHOD ---
        Task<IEnumerable<VendorOrder>?> GetAllOrdersForStoreAsync(int storeId, string vendorId);
        
        // For Delivery Companies
        Task<CustomerOrder?> GetByIdAsync(int orderId);
        Task<IEnumerable<CustomerOrder>> GetUndeliveredOrdersAsync();
    }
}