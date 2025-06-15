using Cloud_Mall.Application.DTOs.StoreCategory;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreCategoryRepository
    {
        Task<List<StoreCategoryDto>> GetAllAsync();
        Task<StoreCategoryDto> CreateAsync(string name, string description);
        Task<Domain.Entities.StoreCategory?> GetById(int id);
    }
}