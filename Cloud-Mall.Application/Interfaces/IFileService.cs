using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveStoreLogoAsync(IFormFile file, string vendorId);
        Task<string> SaveProductImageAsync(IFormFile file, string storeId);
    }
}
