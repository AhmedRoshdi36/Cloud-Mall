using Cloud_Mall.Application.DTOs.GoverningLocation;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Repositories
{
    internal class GoverningLocationRepository : IGoverningLocationRepository
    {
        private readonly ApplicationDbContext context;

        public GoverningLocationRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<GoverningLocationDTO> CreateAsync(string name, string region)
        {
            var location = new GoverningLocation()
            {
                Name = name,
                Region = region
            };
            await context.GoverningLocations.AddAsync(location);
            await context.SaveChangesAsync();
            return new GoverningLocationDTO() { Id = location.ID, Name = name, Region = region };
        }

        public async Task<List<GoverningLocationDTO>> GetAllAsync()
        {
            return await context.GoverningLocations
            .Select(g => new GoverningLocationDTO
            {
                Id = g.ID,
                Name = g.Name,
                Region = g.Region
            })
            .ToListAsync();
        }

        public async Task<bool> AllExistAsync(IEnumerable<int> locationIds)
        {
            // 1. Get the distinct list of IDs from the input
            var distinctIds = locationIds.Distinct().ToList();

            // 2. Query the database to count how many of these IDs actually exist
            var foundCount = await context.GoverningLocations
                                          .CountAsync(gl => distinctIds.Contains(gl.ID));

            // 3. If the number of IDs found in the DB matches the number of
            //    distinct IDs we were looking for, then all of them exist.
            return foundCount == distinctIds.Count;
        }
    }
}
