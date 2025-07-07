using Cloud_Mall.Application.Interfaces.Repositories;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreRepository StoresRepository { get; }
        IStoreCategoryRepository StoreCategoriesRepository { get; }
        IGoverningLocationRepository GoverningLocationsRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IVendorRepository VendorRepository { get; }
        //Add more when needed...
        Task<int> SaveChangesAsync();
    }
}
