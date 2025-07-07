using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddCustomerOrderAsync(CustomerOrder order)
        {
            await _context.CustomerOrders.AddAsync(order);
        }

        public async Task<CustomerOrder?> GetCustomerOrderByIdAsync(int orderId, string clientId)
        {
            return await _context.CustomerOrders
                .Include(co => co.VendorOrders).ThenInclude(vo => vo.Store)
                .Include(co => co.VendorOrders).ThenInclude(vo => vo.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(co => co.ID == orderId && co.ClientID == clientId);
        }

        public async Task<IEnumerable<CustomerOrder>?> GetAllCustomerOrdersAsync(string clientId)
        {
            return await _context.CustomerOrders
                .Where(co => co.ClientID == clientId)
                .Include(co => co.VendorOrders) // <-- Eagerly load the VendorOrders
                    .ThenInclude(vo => vo.Store) // <-- Also get the Store name for the DTO
                .Include(co => co.VendorOrders)
                    .ThenInclude(vo => vo.OrderItems) // <-- Eagerly load the OrderItems
                    .ThenInclude(oi => oi.Product) // <-- Also get the Product name for the DTO
                .OrderByDescending(co => co.OrderDate)
                .ToListAsync();
        }

        public async Task<VendorOrder?> GetVendorOrderByIdAsync(int orderId, string vendorId)
        {
            return await _context.VendorOrders
                .Include(vo => vo.OrderItems).ThenInclude(oi => oi.Product)
                .Include(vo => vo.CustomerOrder.Client)
                .FirstOrDefaultAsync(vo => vo.ID == orderId && vo.Store.VendorID == vendorId);
        }

        public async Task<IEnumerable<VendorOrder>?> GetAllOrdersForVendorAsync(string vendorId)
        {
            return await _context.VendorOrders
                .Where(vo => vo.Store.VendorID == vendorId)
                .Include(vo => vo.Store) // Include Store for the name
                .Include(vo => vo.CustomerOrder) // Include parent...
                    .ThenInclude(co => co.Client) // ...to get Client details
                .Include(vo => vo.OrderItems) // Include items...
                    .ThenInclude(oi => oi.Product) // ...to get Product details
                .OrderByDescending(vo => vo.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VendorOrder>?> GetAllOrdersForStoreAsync(int storeId, string vendorId)
        {
            var storeIsOwnedByVendor = await _context.Stores
                .AnyAsync(s => s.ID == storeId && s.VendorID == vendorId);

            if (!storeIsOwnedByVendor)
            {
                return null;
            }

            return await _context.VendorOrders
                .Where(vo => vo.StoreID == storeId)
                .Include(vo => vo.Store) // <-- FIX: Eagerly load the Store to get its name
                .Include(vo => vo.CustomerOrder) // <-- FIX: Eagerly load the parent order...
                    .ThenInclude(co => co.Client) // <-- ...to get the Client's details
                .Include(vo => vo.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(vo => vo.OrderDate)
                .ToListAsync();
        }
    }
}