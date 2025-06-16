using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Infrastructure.Repositories;

namespace Cloud_Mall.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public IStoreRepository StoresRepository { get; private set; }

        public IStoreCategoryRepository StoreCategoriesRepository { get; private set; }

        public IGoverningLocationRepository GoverningLocationsRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;

            // Create instances of your concrete repositories, passing in the shared DbContext.
            StoresRepository = new StoreRepository(context);
            StoreCategoriesRepository = new StoreCategoryRepository(context);
            GoverningLocationsRepository = new GoverningLocationRepository(context);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
