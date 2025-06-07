using Cloud_Mall.Application.DTOs.StoreCategory;
using MediatR;

namespace Cloud_Mall.Application.StoreCategory.Command.CreateStoreCategory
{
    public class CreateStoreCategoryCommand : IRequest<StoreCategoryDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
