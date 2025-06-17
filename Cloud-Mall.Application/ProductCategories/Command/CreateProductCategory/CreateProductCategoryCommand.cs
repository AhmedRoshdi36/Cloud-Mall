using MediatR;

namespace Cloud_Mall.Application.ProductCategories.Command.CreateProductCategory
{
    public class CreateProductCategoryCommand : IRequest<ApiResponse<int>>
    {
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}