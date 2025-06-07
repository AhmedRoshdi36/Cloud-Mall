using Cloud_Mall.Application.DTOs.StoreCategory;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreCategoryRepository
    {
        Task<IEnumerable<Domain.Entities.StoreCategory>> GetAllAsync();
        Task<StoreCategoryDto> CreateAsync(string name, string description);
    }
}