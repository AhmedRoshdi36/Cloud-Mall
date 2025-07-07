

using Cloud_Mall.Domain.Entities;

namespace Cloud_Mall.Application.Interfaces;

public interface IVendorRepository
{
    Task<List<ApplicationUser>> GetAllVendorsAsync();

}
