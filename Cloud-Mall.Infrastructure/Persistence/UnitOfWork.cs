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

        public UnitOfWork(
            ApplicationDbContext context,
            IStoreRepository storesRepository,
            IStoreCategoryRepository storeCategoriesRepository,
            IGoverningLocationRepository governingLocationsRepository,
            IProductCategoryRepository productCategoryRepository
            )
        {
            this.context = context;

            StoresRepository = storesRepository;
            StoreCategoriesRepository = storeCategoriesRepository;
            GoverningLocationsRepository = governingLocationsRepository;
            ProductCategoryRepository = productCategoryRepository;
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