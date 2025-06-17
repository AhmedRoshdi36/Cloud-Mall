using Cloud_Mall.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Infrastructure.Services.FileService
{
    internal class FileService : IFileService
    {
        private readonly string _rootStorePath;
        private readonly string _rootProductPath;

        public FileService()
        {
            // Note: It's generally better to use IWebHostEnvironment to get the wwwroot path
            // but for consistency with your original code, Directory.GetCurrentDirectory() is used.
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _rootStorePath = Path.Combine(wwwrootPath, "stores");
            _rootProductPath = Path.Combine(wwwrootPath, "products");

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

        /// <summary>
        /// Deletes a product image from the wwwroot/products folder.
        /// </summary>
        /// <param name="imagePath">The relative path of the image (e.g., /products/image.jpg).</param>
        public void DeleteProductImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return;
            }

            var fileName = Path.GetFileName(imagePath);
            var filePath = Path.Combine(_rootProductPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Deletes a store logo from the wwwroot/stores folder.
        /// </summary>
        /// <param name="logoPath">The relative path of the logo (e.g., /stores/logo.png).</param>
        public void DeleteStoreLogo(string logoPath)
        {
            if (string.IsNullOrEmpty(logoPath))
            {
                return;
            }

            var fileName = Path.GetFileName(logoPath);
            var filePath = Path.Combine(_rootStorePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}