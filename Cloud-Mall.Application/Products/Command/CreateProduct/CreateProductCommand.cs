using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Application.Products.Command.CreateProduct
{
    public class CreateProductCommand : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int Stock { get; set; }
        public IFormFile Image { get; set; }
        public int StoreID { get; set; }
        public int ProductCategoryID { get; set; }
    }
}
