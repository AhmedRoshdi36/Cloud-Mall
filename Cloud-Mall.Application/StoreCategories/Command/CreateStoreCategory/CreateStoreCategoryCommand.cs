using Cloud_Mall.Application.DTOs.StoreCategory;
using MediatR;

namespace Cloud_Mall.Application.StoreCategories.Command.CreateStoreCategory
{
    public class CreateStoreCategoryCommand : IRequest<ApiResponse<StoreCategoryDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
