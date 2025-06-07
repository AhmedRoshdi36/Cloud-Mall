using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Services.StoreCategoryService
{
    internal class StoreCategoryRepository : IStoreCategoryRepository
    {
        private readonly ApplicationDbContext context;

        public StoreCategoryRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<StoreCategoryDto> CreateAsync(string name, string description)
        {
            var category = new StoreCategory { Name = name, Description = description };
            await context.StoreCategories.AddAsync(category);
            await context.SaveChangesAsync();
            return new StoreCategoryDto { Id = category.ID, Name = name, Description = description };
        }

        public async Task<IEnumerable<StoreCategory>> GetAllAsync()
        {
            return await context.StoreCategories.ToListAsync();
        }
    }
}
