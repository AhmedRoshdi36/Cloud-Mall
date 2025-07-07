using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Application.Interfaces.Repositories;

namespace Cloud_Mall.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public IStoreRepository StoresRepository { get; private set; }
        public IStoreCategoryRepository StoreCategoriesRepository { get; private set; }
        public IGoverningLocationRepository GoverningLocationsRepository { get; private set; }
        public IProductCategoryRepository ProductCategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICartRepository CartRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public IVendorRepository VendorRepository { get; private set; }

        public UnitOfWork(
            ApplicationDbContext context,
            IStoreRepository storesRepository,
            IStoreCategoryRepository storeCategoriesRepository,
            IGoverningLocationRepository governingLocationsRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IVendorRepository vendorRepository
            )
        {
            this.context = context;

            StoresRepository = storesRepository;
            StoreCategoriesRepository = storeCategoriesRepository;
            GoverningLocationsRepository = governingLocationsRepository;
            ProductCategoryRepository = productCategoryRepository;
            ProductRepository = productRepository;
            CartRepository = cartRepository;
            OrderRepository = orderRepository;
            VendorRepository =vendorRepository;
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}