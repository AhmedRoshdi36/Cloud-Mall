using Cloud_Mall.Application.DTOs.GoverningLocation;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IGoverningLocationRepository
    {
        Task<GoverningLocationDTO> CreateAsync(string name, string region);
        Task<List<GoverningLocationDTO>> GetAllAsync();
    }
}
