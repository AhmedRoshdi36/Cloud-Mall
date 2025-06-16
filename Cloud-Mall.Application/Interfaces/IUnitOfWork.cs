namespace Cloud_Mall.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreRepository StoresRepository { get; }
        IStoreCategoryRepository StoreCategoriesRepository { get; }
        IGoverningLocationRepository GoverningLocationsRepository { get; }
        //Add more when needed...
        Task<int> SaveChangesAsync();
    }
}
