using Cloud_Mall.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace Cloud_Mall.Infrastructure.Services.FileService
{
    internal class FileService : IFileService
    {
        private readonly string _rootStorePath;
        private readonly string _rootProductPath;

        public FileService(IConfiguration configuration)
        {
            _rootStorePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "stores");
            _rootProductPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products");
            Directory.CreateDirectory(_rootStorePath); // Ensure directory exists
            Directory.CreateDirectory(_rootProductPath); // Ensure directory exists
        }
        public async Task<string> SaveProductImageAsync(IFormFile file, string storeId)
        {
            var fileName = $"{storeId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_rootProductPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/products/{fileName}";
        }

        public async Task<string> SaveStoreLogoAsync(IFormFile file, string vendorId)
        {
            var fileName = $"{vendorId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_rootStorePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/stores/{fileName}";
        }
    }
}
