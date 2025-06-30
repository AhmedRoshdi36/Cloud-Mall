using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IStoreCategoryRepository
    {
        Task<List<StoreCategoryDto>> GetAllAsync();
        Task<StoreCategory> CreateAsync(string name, string description);
        Task<StoreCategory?> GetById(int id);
    }
}